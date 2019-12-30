clear all
clc

load u.data

[ newValues ] = PrepareDataSet(u);

[row,col]= size(newValues);
colIndex = 1;
foldSize = round(colIndex + (col/10));

fold = zeros(1,1);
errRates = zeros(1,1);
userIndex = 1;
count = 1;
for knnVal = 10 : 10 : 90
    colIndex = 1;
    kFoldEnd = (colIndex+foldSize);
    fold = zeros(1,1);
    ratedIndices = zeros(1,1);
    for i = 1 : 10
        [ testSet , trainSet ] = PartitionMatrices(newValues,colIndex,kFoldEnd);
        [testRow,testCol] = size(testSet);
        summ = 0;
        for j = 1 : testCol
            itemRates = testSet(:,j);
            rateIndices = find(itemRates ~= -1);
            ratedIndices(i,j) = rateIndices(1,1);
            userIndex = rateIndices(1,1);
            [ sortedSimilarity,newGuess] = ItemBased(trainSet,itemRates,userIndex,knnVal);
            fold(i,j) = round(newGuess);
        end
        
        ratedCount = 0;
        for j = 1 : testCol
            rateVal = fold(i,j);
            if(isnan(rateVal))
                continue;
            end
            summ = summ + abs(rateVal - testSet(ratedIndices(i,j),j));
            ratedCount = ratedCount + 1;
        end

        %calculate the error rate for the current i'th fold and knnValue
        errRates(count,i) = summ / ratedCount;

        colIndex = foldSize*i;
        kFoldEnd = colIndex + foldSize;
    end
    count = count+1;
end

%userRates = newValues(1,:);

%withOutUser = newValues(2:943,:);

%[ distanceValues, bestSimilarities , newGuess] = KNNCalculation(withOutUser,userRates,3,10);

%% partition
function [ testSet, trainSet ] = PartitionMatrices(input, rowIndex, KFoldEnd)


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