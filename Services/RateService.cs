using CreateModel;
using KNNCalculation;
using MathWorks.MATLAB.NET.Arrays;
using Microsoft.EntityFrameworkCore;
using MovieRecommender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRecommender.Services
{
    public class RateService : IRatingService
    {
        private readonly MovieContext movieContext;

        private readonly CalculateKNN calculateKNN;

        private readonly PredictionHelper.PredictionHelper predictionHelper;

        private readonly CreateTrainingModel createModel;

        public RateService(MovieContext movieContext, CalculateKNN calculateKNN, PredictionHelper.PredictionHelper predictionHelper, CreateTrainingModel createTraining)
        {
            this.movieContext = movieContext;
            this.calculateKNN = calculateKNN;
            this.predictionHelper = predictionHelper;
            this.createModel = createTraining;
        }

        public async Task AddBulkRating(IEnumerable<Rate> rates)
        {
            await this.movieContext.Rates.AddRangeAsync(rates);
            await this.movieContext.SaveChangesAsync();
        }

        public async Task AddRating(Rate rate)
        {
            await this.movieContext.Rates.AddAsync(rate);
            await this.movieContext.SaveChangesAsync();
        }

        public async Task<List<Rate>> GetRates()
        {
            return await this.movieContext.Rates.ToListAsync();
        }

        public async Task<RateResult> PredictMovieRate(User predictedUser, int movieIndex)
        {
            MWArray[] models = createModel.CreateModel(2);

            var userModel = models[0];
            var itemModel = models[1];

            List<Rate> rates = await GetRates();
            int[,] rateArray = new int[rates.Count - predictedUser.Rates.Count, 4];

            var userCount = await movieContext.Users.CountAsync();

            int[,] itemRates = new int[userCount, 1];

            //Convert rates to input data set
            for (int i = 0; i < rates.Count; i++)
            {
                var currentRate = rates[i];

                //discard user's rate
                if (currentRate.UserId == predictedUser.Id)
                    continue;

                if (currentRate.MovieId == movieIndex)
                {
                    itemRates[currentRate.UserId - 1, 0] = currentRate.MovieRate;
                }

                rateArray[i, 0] = currentRate.UserId;
                rateArray[i, 1] = currentRate.MovieId;
                rateArray[i, 2] = currentRate.MovieRate;
            }

            for (int i = 0; i < itemRates.Length; i++)
            {
                if (itemRates[i, 0] == 0)
                    itemRates[i, 0] = -1;
            }

            //Create user-item dataset for calculating knn
            MWArray newValues = calculateKNN.PrepareDataSet((MWNumericArray)rateArray);

            double[,] newRates = (double[,])newValues.ToArray();

            //create a given user's rate matrices for comparing values
            var movieCount = newRates.GetLength(1);
            double[,] userRates = new double[1, movieCount];
            var predictedUserRates = predictedUser.Rates;
            for (int i = 0; i < movieCount; i++)
            {
                userRates[0, i] = -1;
            }

            predictedUserRates.ForEach(rate => userRates[0, rate.MovieId] = rate.MovieRate);

            // trainSet, movieRates , userToGuessIndex, N
            MWArray[] itemResult = predictionHelper.ItemBased(2, itemModel, (MWNumericArray)itemRates, predictedUser.Id - 1, 60);

            var itemPrediction = (double[,])itemResult[1].ToArray();

            var roundedItem = -1;
            var itemVal = itemPrediction[0, 0];
            if (!Double.IsNaN(itemVal))
                roundedItem = Convert.ToInt32(itemVal);

            //RESULT
            //mw[0] = distanceValues
            //mw[1] = bestSimilartityValues
            //mw[2] = guess for selected movie
            MWArray[] mW = predictionHelper.KNNCalculation(3, userModel, (MWNumericArray)userRates, movieIndex, 50);

            var userPrediction = (double[,])mW[2].ToArray();

            var value = userPrediction[0, 0];
            int roundedUser = -1;
            if (!Double.IsNaN(value))
            {
                roundedUser = Convert.ToInt32(value);
            }
            return new RateResult { ItemResult = roundedItem, UserResult = roundedUser };
        }
    }

    public class RateResult
    {
        public int ItemResult { get; set; }

        public int UserResult { get; set; }

        public string Message { get; set; }
    }
}
