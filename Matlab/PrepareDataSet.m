%% This will take all the rates and prepare user-item rate dataset.

function [ dataset ] = PrepareDataSet( u )
[user, ~] = size(u);

OriginalDataset = zeros(1,1);
for i = 1 : user
    OriginalDataset(u(i,1),u(i,2)) = u(i,3);
end

OriginalDataset(~OriginalDataset) = -1;

dataset = OriginalDataset;

end