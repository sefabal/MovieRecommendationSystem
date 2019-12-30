clear all
clc

load u.data

[ newValues ] = PrepareDataSet(u);

[row,col]= size(newValues);
rowIndex = 1;
foldSize = round(rowIndex + (row/10));

fold = zeros(1,1);
errRates = zeros(1,1);
movieIndex = 1;
count = 1;
for knnVal = 10 : 10 : 90
    rowIndex = 1;
    kFoldEnd = (rowIndex+foldSize);
    fold = zeros(1,1);
    for i = 1 : 10
        [ testSet , trainSet ] = PartitionMatrices(newValues,rowIndex,kFoldEnd);
        [testRow,~] = size(testSet);
        summ = 0;
        ratedIndices = zeros(1,1);
        for j = 1 : testRow
            userRates = testSet(j,:);
            rateIndices = find(userRates ~= -1);
            ratedIndices(i,j) = rateIndices(1,1);
            [ ~, ~ , newGuess] = KNNCalculation(trainSet,userRates,movieIndex,knnVal);
            fold(i,j) = round(newGuess);
        end

        for j = 1 : testRow
            summ = summ + abs(fold(i,j) - testSet(j,ratedIndices(i,j)));
        end

        %calculate the error rate for the current i'th fold and knnValue
        errRates(count,i) = summ / testRow;

        rowIndex = foldSize*i;
        kFoldEnd = rowIndex + foldSize;
    end
    count = count+1;
end

%userRates = newValues(1,:);

%withOutUser = newValues(2:943,:);

%[ distanceValues, bestSimilarities , newGuess] = KNNCalculation(withOutUser,userRates,3,10);

%% partition
function [ testSet, trainSet ] = PartitionMatrices(input, rowIndex, KFoldEnd)


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