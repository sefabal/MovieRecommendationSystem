function [ userTrainSet,itemTrainSet ] = CreateModel()

load u.data u

[ newValues ] = PrepareDataSet(u);

% k = 50 , 3. fold
[row,col]= size(newValues);
userfoldSize = round(1+(row/10));

% k = 60 , 4. fold
itemfoldSize = round(1 + (col/10));
colIndex = itemfoldSize*4;
itemkFoldEnd = colIndex + itemfoldSize;

rowIndex = userfoldSize*3;z
userkFoldEnd = rowIndex + userfoldSize;

[ ~ , userTrainSet ] = PartitionUserMatrices(newValues,rowIndex,userkFoldEnd);

[ ~ , itemTrainSet ] = PartitionItemMatrices(newValues,colIndex,itemkFoldEnd);

end
%% partition
function [ testSet, trainSet ] = PartitionUserMatrices(input, rowIndex, KFoldEnd)

[row,col] = size(input);

testSet = zeros(1,col);
trainSet = zeros(1,col);

testCount = 1;
trainCount = 1;
for i = 1: row
   if (i >= rowIndex && i <= KFoldEnd)
       testSet(testCount,:) = input(i,:);
       testCount = testCount+1;
   else
       trainSet(trainCount,:) = input(i,:);
       trainCount = trainCount +1;
   end
end
end
%% partition
function [ testSet, trainSet ] = PartitionItemMatrices(input, rowIndex, KFoldEnd)


[row,col] = size(input);

testSet = zeros(row,1);
trainSet = zeros(row,1);

testCount = 1;
trainCount = 1;
for i = 1: col
   if (i >= rowIndex && i <= KFoldEnd)
       testSet(:,testCount) = input(:,i);
       testCount = testCount+1;
   else
       trainSet(:,trainCount) = input(:,i);
       trainCount = trainCount +1;
   end
end

end