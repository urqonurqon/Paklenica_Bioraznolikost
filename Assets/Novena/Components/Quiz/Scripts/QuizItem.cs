namespace Scripts.Quiz {
	public class QuizItem {
		//Properties + Getters & Setters

		string _explanation;

		
		public string Explanation
		{
			get { return _explanation; }
			set { _explanation = value; }
		}

		string _answer1;
		public string Answer1
		{
			get { return _answer1; }
			set { _answer1 = value; }
		}

		string _answer2;
		public string Answer2
		{
			get { return _answer2; }
			set { _answer2 = value; }
		}

		string _answer3;
		public string Answer3
		{
			get { return _answer3; }
			set { _answer3 = value; }
		}

		string _answer4;
		public string Answer4
		{
			get { return _answer4; }
			set { _answer4 = value; }
		}

		string _question;
		public string Question
		{
			get { return _question; }
			set { _question = value; }
		}

		string _imagePath1;
		string _imagePath2;
		string _imagePath3;
		string _imagePath4;
		public string ImagePath1
		{
			get { return _imagePath1; }
			set { _imagePath1 = value; }
		}
		public string ImagePath2
		{
			get { return _imagePath2; }
			set { _imagePath2 = value; }
		}
		public string ImagePath3
		{
			get { return _imagePath3; }
			set { _imagePath3 = value; }
		}
		public string ImagePath4
		{
			get { return _imagePath4; }
			set { _imagePath4 = value; }
		}

		int _rightAnswerIndicator;
		public int RightAnswerIndicator
		{
			get { return _rightAnswerIndicator; }
			set { _rightAnswerIndicator = value; }
		}

		// Constructor
		public QuizItem(string question, int rightAnswerIndicator, string explanation, string answer1 = "", string imagePath1 = "", string answer2 = "", string imagePath2 = "", string answer3 = "", string imagePath3 = "", string answer4 = "", string imagePath4 = "")
		{
			_explanation = explanation;
			_answer1 = answer1;
			_answer2 = answer2;
			_answer3 = answer3;
			_answer4 = answer4;
			_question = question;
			_imagePath1 = imagePath1;
			_imagePath2 = imagePath2;
			_imagePath3 = imagePath3;
			_imagePath4 = imagePath4;
			_rightAnswerIndicator = rightAnswerIndicator;
		}
	}
}