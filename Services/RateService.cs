using KNNCalculation;
using MathWorks.MATLAB.NET.Arrays;
using Microsoft.EntityFrameworkCore;
using MovieRecommender.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRecommender.Services
{
    public class RateService : IRatingService
    {
        private readonly MovieContext movieContext;

        private readonly CalculateKNN calculateKNN;

        public RateService(MovieContext movieContext, CalculateKNN calculateKNN)
        {
            this.movieContext = movieContext;
            this.calculateKNN = calculateKNN;
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

        public async Task<int> PredictMovieRate(User predictedUser, int movieIndex)
        {
            List<Rate> rates = await GetRates();
            int[,] rateArray = new int[rates.Count-predictedUser.Rates.Count, 4];

            //Convert rates to input data set
            for (int i = 0; i < rates.Count; i++)
            {
                var currentRate = rates[i];

                if (currentRate.UserId == predictedUser.Id)
                    continue;

                rateArray[i, 0] = currentRate.UserId;
                rateArray[i, 1] = currentRate.MovieId;
                rateArray[i, 2] = currentRate.MovieRate;
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

            //RESULT
            //mw[0] = distanceValues
            //mw[1] = bestSimilartityValues
            //mw[2] = guess for selected movie
            MWArray[] mW = calculateKNN.KNNCalculation(3, newValues, (MWNumericArray)userRates, movieIndex, 30);
            
            var predictedVal = (double[,])mW[2].ToArray();

            var value = predictedVal[0, 0];

            var rounded = Convert.ToInt32(value);

            return rounded;

        }


    }
}
