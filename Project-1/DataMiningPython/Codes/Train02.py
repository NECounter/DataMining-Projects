import tensorflow as tf
import pandas as pd

tf.logging.set_verbosity(tf.logging.INFO)

project_name = "AgePredict1"
train_csv_file = "C:\MediaSlot\CloudDocs\Docs\ds\T06BlackFriday-Regulated-Train.csv"
test_csv_file = "C:\MediaSlot\CloudDocs\Docs\ds\T06BlackFriday-Regulated-Test.csv"
image_resize = [8]


def model_fn(features, labels, mode, params):
    features_net = tf.feature_column.input_layer(features, params['feature_columns'])
    dense_1544193670052 = tf.layers.dense(inputs=features_net, units=1024, activation=tf.nn.relu)

    dense_1544193653559 = tf.layers.dense(inputs=dense_1544193670052, units=1024, activation=tf.nn.relu)

    dense_1544195962540 = tf.layers.dense(inputs=dense_1544193653559, units=1024, activation=tf.nn.relu)

    logits = dense_1544195962540

    predictions = {
        "classes": tf.argmax(input=logits, axis=1),
        "probabilities": tf.nn.softmax(logits, name="softmax_tensor")
    }
    # Prediction and training
    if mode == tf.estimator.ModeKeys.PREDICT:
        return tf.estimator.EstimatorSpec(mode=mode, predictions=predictions)

    # Calculate Loss (for both TRAIN and EVAL modes)
    onehot_labels = tf.one_hot(indices=tf.cast(labels, tf.int32), depth=1024)
    loss = tf.losses.softmax_cross_entropy(
        onehot_labels=onehot_labels, logits=logits)

    # Compute evaluation metrics.
    accuracy = tf.metrics.accuracy(labels=labels,
                                   predictions=predictions["classes"],
                                   name='acc_op')
    metrics = {'accuracy': accuracy}
    tf.summary.scalar('accuracy', accuracy[1])

    # Configure the Training Op (for TRAIN mode)
    if mode == tf.estimator.ModeKeys.TRAIN:
        optimizer = tf.train.GradientDescentOptimizer(learning_rate=0.001)
        train_op = optimizer.minimize(
            loss=loss,
            global_step=tf.train.get_global_step())
        return tf.estimator.EstimatorSpec(mode=mode, loss=loss, train_op=train_op)

    # Add evaluation metrics (for EVAL mode)
    eval_metric_ops = {
        "accuracy": tf.metrics.accuracy(
            labels=labels, predictions=predictions["classes"])}
    return tf.estimator.EstimatorSpec(
        mode=mode, loss=loss, eval_metric_ops=eval_metric_ops)


# Parse CSV file
def load_data():
    train = pd.read_csv(train_csv_file, header=0)
    labels = train.pop("Age")
    features = train

    test = pd.read_csv(test_csv_file, header=0)
    labels_test = test.pop("Age")
    features_test = test

    return (features, labels), (features_test, labels_test)


def train_input_fn(features, labels):
    """An input function for training"""
    # Convert the inputs to a Dataset.
    dataset = tf.data.Dataset.from_tensor_slices((dict(features), labels))
    # Shuffle, repeat, and batch the examples.
    dataset = dataset.shuffle(1000)
    dataset = dataset.repeat().batch(100)

    # Return the dataset.
    return dataset


def eval_input_fn(features, labels):
    """An input function for training"""
    # Convert the inputs to a Dataset.
    dataset = tf.data.Dataset.from_tensor_slices((dict(features), labels))
    dataset = dataset.batch(100)
    # return the dataset
    return dataset


def main(unused_argv):
    (train_features, train_labels), (test_features, test_labels) = load_data()
    # Feature columns describe how to use the input.
    my_feature_columns = []
    for key in train_features.keys():
        my_feature_columns.append(tf.feature_column.numeric_column(key=key))

    # Create the Estimator
    classifier = tf.estimator.Estimator(
        model_fn=model_fn,
        model_dir="/tmp/" + project_name,
        params={
            'feature_columns': my_feature_columns
        }
    )

    classifier.train(input_fn=lambda: train_input_fn(train_features, train_labels), steps=10000)

    eval_results = classifier.evaluate(input_fn=lambda: eval_input_fn(test_features, test_labels))
    tf.summary.scalar("Accuracy", eval_results["accuracy"])
    print(eval_results)


if __name__ == "__main__":
    tf.app.run()