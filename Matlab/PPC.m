%%It takes two matrices and find their common rated movies for finding
%%their similarity, return a pcc value

function [ pcc ] = PPC(A,B)
%first rated movie indices and second rated movie indices
firstSampleIndices = find(A ~= -1);
secondSampleIndices = find(B ~= -1);

[commonMoviesIndices,~,~] = intersect(firstSampleIndices,secondSampleIndices);
[~,col] = size(commonMoviesIndices);

firstSample = zeros(1,1);
secondSample = zeros(1,1);
for i = 1 : col
    firstSample(1,i) = A(1,commonMoviesIndices(1,i));
    secondSample(1,i) = B(1,commonMoviesIndices(1,i));
end

%calculate user's all rates mean not the intersection
firstMean = CalcMean(transpose(A));
secondMean = CalcMean(transpose(B));

summation = 0;
aSum = 0;
bSum = 0;
for i = 1 : col 
   currentIndex = commonMoviesIndices(1,i);
   summation = summation + ((A(1,currentIndex) - firstMean) * (B(1,currentIndex) - secondMean));
   aSum = aSum + (A(1,currentIndex) - firstMean)^2;
   bSum = bSum + (B(1,currentIndex) - secondMean)^2;
end

pcc = summation / (aSum^(1/2) * bSum^(1/2));

% significance weighting
if(col < 50)
    pcc = (col/50) * pcc;
end

end

%% Calculate the mean of the given matrix
function [ mean ] = CalcMean(x)
[satir ,~] = size(x);
count = 0;
totalMean = 0;
for i = 1 : satir
    if (x(i,1)) ~= -1
        totalMean = totalMean + x(i,1);
        count = count+1;
    end
end
mean = totalMean / count;
end

%% Calculate standard deviation
function [ std ] = CalcStd(x)
[satir,~] = size(x);
mean = CalcMean(x);
sumOfSquares = 0;
count = 0;
for i = 1 : satir
    if(x(i,1) ~= -1)
        sumOfSquares = sumOfSquares + (mean - x(i,1))^2;
        count = count + 1;
    end
end

if( count ~= 1)
    variance = sumOfSquares / (count-1);
    std = sqrt(variance);
else
    std = 0;
end
end