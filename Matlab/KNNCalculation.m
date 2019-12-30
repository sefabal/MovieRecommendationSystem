%%  take train set and use k closest neighbor algorith to classify

% should create matrice for user that includes all the movies even if it is not voted(-1)
% before calling the method
function [ distanceValues ,sortedSimilarity, newGuesses] = KNNCalculation(trainSet,userRates,movieToGuessIndex,N)
    
[row,~] = size(trainSet);

% calculate and compare each user's movie rates with each other user
distanceValues = zeros(1,1);
for i = 1 : row
    firstSample = trainSet(i,:);
    distance = PPC(firstSample,userRates);
    distanceValues(i) = distance;
end

% take 40 closest neighbor and their indices
[sortedSimilarities,users] = GetNClosest(distanceValues(1,:),N);
sortedSimilarity = sortedSimilarities;
[testRow, ~] = size(users);
for i = 1 : testRow
    % now we have 40 closest user
    
    %guess one movie rates with these closest users
    % Take guessed movie's indices and create a new rate matrix and take mod
    % 3 movie to guess
  
    rates = zeros(1,1);
    [~,indice] = size(users);
    count = 1;
    for user = 1 : indice
       userIndex = users(1,user);
       
       if(isnan(userIndex) || userIndex < 0 || userIndex > row)
          continue;
       end
       rate = trainSet(userIndex,movieToGuessIndex);
       
       rates(1,count) = rate;
       count = count + 1;
    end
    
    % not take mode we should predict the value
    % user mean + sim*rate / sim..
    [ predicted ] = PredictValue(rates,sortedSimilarities,users,trainSet);
    predictedVal = CalcMean(transpose(userRates)) + predicted;
    guess = predictedVal;
end

newGuesses = round(guess);
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

%%

% user mean + sim*rate / sim..
function [ predicted ] = PredictValue( rates,distanceValues,users,newValues )

[~,rateCol] = size(rates);

prediction = 0;
similarities = 0;
for i = 1: rateCol
    if(rates(1,i) == -1)
        continue
    end
    prediction = prediction + distanceValues(1,i)*(rates(1,i)-CalcMean(transpose(newValues(users(1,i),:))));
    similarities = similarities + distanceValues(1,i);
end

predicted = (prediction / similarities);
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
