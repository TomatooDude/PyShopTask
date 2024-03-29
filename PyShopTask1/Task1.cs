﻿using System;


namespace Task1
{
	class App
	{
		static void Main (string[] args)
		{
			//Game.task1();
		}
	}
	public struct Score
	{
		public int home;
		public int away;

		public Score (int home, int away)
		{
			this.home = home;
			this.away = away;
		}
	}

	public struct GameStamp
	{
		public int offset;
		public Score score;

		public GameStamp (int offset, int home, int away)
		{
			this.offset = offset;
			this.score = new Score(home, away);
		}
	}
	public class Game
	{
		const int TIMESTAMPS_COUNT = 5;

		const double PROBABILITY_SCORE_CHANGED = 0.0001;

		const double PROBABILITY_HOME_SCORE = 0.45;

		const int OFFSET_MAX_STEP = 3;

		GameStamp[] gameStamps;

		public Game ()
		{
			this.gameStamps = new GameStamp[] { };
		}

		public Game (GameStamp[] gameStamps)
		{
			this.gameStamps = gameStamps;
		}

		GameStamp generateGameStamp (GameStamp previousValue)
		{
			Random rand = new Random();

			bool scoreChanged = rand.NextDouble() > 1 - PROBABILITY_SCORE_CHANGED;
			int homeScoreChange = scoreChanged && rand.NextDouble() > 1 - PROBABILITY_HOME_SCORE ? 1 : 0;
			int awayScoreChange = scoreChanged && homeScoreChange == 0 ? 1 : 0;
			int offsetChange = (int)(Math.Floor(rand.NextDouble() * OFFSET_MAX_STEP)) + 1;

			return new GameStamp(
				previousValue.offset + offsetChange,
				previousValue.score.home + homeScoreChange,
				previousValue.score.away + awayScoreChange
				);
		}

		static Game generateGame ()
		{
			Game game = new Game();
			game.gameStamps = new GameStamp[TIMESTAMPS_COUNT];


			GameStamp currentStamp = new GameStamp(0, 0, 0);
			for (int i = 0; i < TIMESTAMPS_COUNT; i++)
			{
				game.gameStamps[i] = currentStamp;
				currentStamp = game.generateGameStamp(currentStamp);
			}

			return game;
		}

		public static void task1 ()
		{
			//int a = 1;
			Game game = generateGame();
			game.printGameStamps();
			//game.GetScore(a);
		}

		void printGameStamps ()
		{
			foreach (GameStamp stamp in this.gameStamps)
			{
				Console.WriteLine($"{stamp.offset}: {stamp.score.home}-{stamp.score.away}");
			}
		}


		/// <summary>
		/// Выводит счет на момент времени offset.
		/// </summary>
		/// <param name="offset"></param>
		/// <returns></returns>

		public Score GetScore (int offset)
		{
			Score score = new Score();
			GameStamp[] stamp = this.gameStamps;

			try
			{
				for (int count = 0; count <= stamp.Length;)
				{
					var value = stamp[count];

					if ( offset<0) // need case
					{
						throw new IndexOutOfRangeException("Incorrect number, please re-enter");
					}
					if (value.offset < offset) // need case
					{
						count++;
					}

					//if (value.offset < offset && value.offset != offset) //
					//{
					//	throw new IndexOutOfRangeException("Incorrect number, please re-enter");
					//} 

					if (value.offset == offset)
					{
						int scoreHome = value.score.home;
						int scoreAway = value.score.away;
						Console.WriteLine($"Score at the moment {offset} : {scoreHome}, {scoreAway}");

						return new Score(scoreHome, scoreAway);
					}

					if (value.offset > offset && value.offset != offset)
					{
						int scoreHome = value.score.home;
						int scoreAway = value.score.away;
						Console.WriteLine("Score at the moment not found");
						Console.WriteLine($"Score for the next moment {value.offset} : {scoreHome}, {scoreAway}");

						return new Score(scoreHome, scoreAway);
					}
				}

			}
			catch (IndexOutOfRangeException)
			{
				throw new IndexOutOfRangeException("Incorrect number, please re-enter");
			}
			return score;
		}

	}
}