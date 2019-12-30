function [ sortedSimilarity,prediction ] = ItemBased( trainSet, movieRates , userToGuessIndex, N)

[row,col] = size(trainSet);

% calculate and compare each user's movie rates with each other user
distanceValues = zeros(1,1);
for i = 1 : col
    firstSample = trainSet(:,i);
    distance = CosineSim(firstSample,movieRates);
    distanceValues(i) = distance;
end

% take 40 closest neighbor and their indices
[sortedSimilarities,items] = GetNClosest(distanceValues(1,:),N);
sortedSimilarity = sortedSimilarities;

[ predicted ] = PredictValue(items,sortedSimilarities,trainSet,userToGuessIndex);
guess = predicted;

prediction = round(guess);

end

%%

function [ similarity ] = CosineSim( A, B)

A = transpose(A);
B = transpose(B);

firstSampleIndices = find(A ~= -1);
secondSampleIndices = find(B ~= -1);

[commonMoviesIndices,~,~] = intersect(firstSampleIndices,secondSampleIndices);
[row,col] = size(commonMoviesIndices);

firstSample = zeros(1,1);
secondSample = zeros(1,1);
for i = 1 : col
    firstSample(1,i) = A(1,commonMoviesIndices(1,i));
    secondSample(1,i) = B(1,commonMoviesIndices(1,i));
end

% cos sim
% dot prod / lengths

summation = sum(firstSample .* secondSample);
aSum = sum(firstSample .* firstSample);
bSum = sum(secondSample .* secondSample);

similarity = summation / (aSum^(1/2) * bSum^(1/2));

if (col < 50)
    similarity = similarity * (col/50);
end
end

%%
% user mean + sim*rate / sim..
function [ predicted ] = PredictValue( items,distanceValues,trainSet,userToGuessIndex)

[~,itemSize] = size(items);

prediction = 0;
similarities = 0;
for i = 1: itemSize
    guessedUserRate = trainSet(userToGuessIndex,items(1,i));
    if(guessedUserRate == -1)
        continue
    end
    prediction = prediction + distanceValues(1,i)*(guessedUserRate);
    similarities = similarities + distanceValues(1,i);
end

predicted = (prediction / similarities);
end

%% 40 closest point
function [ sorted , indices ] = GetNClosest( A,N )
[sorted,indices]= sort(A,'descend');

[~,col] = size(sorted);

newSorted = zeros(1,1);
newIndices = zeros(1,1);
count = 1;
for i = 1 : col
    if count == N
        break
    end
    if isnan(sorted(1,i))
        continue
    else
        newSorted(1,count) = sorted(1,i);
        newIndices(1,count) = indices(1,i);
        count = count + 1;
    end
end

sorted = newSorted;
indices = newIndices;
end

