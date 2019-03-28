b = load('C:\MediaSlot\CloudDocs\Docs\¿Î³Ì\Data Mining\Records\Iteration5\181206213016.txt');
figure;
hold on;
plot(b(1,:));
plot(b(2,:));
title('Result of 5 Clusters');

ylabel('Amount');
xlabel('Iteration Times');
plot(b(3,:));
plot(b(4,:));
plot(b(5,:));
legend('Cluster 1','Cluster 2','Cluster 3','Cluster 4','Cluster 5');


%plot(b(6,:));
%plot(b(7,:));
%plot(b(8,:));
%plot(b(9,:));
%plot(b(10,:));
hold off;