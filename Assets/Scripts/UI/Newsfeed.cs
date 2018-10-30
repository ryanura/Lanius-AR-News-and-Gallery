using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Newsfeed {

	public int Id{
		get;
		set;
	}

	public Dictionary <string,string> Names
	{
		get;
		set;
	}
		
	public class News
	{
		private const string KEY_ID  = "id";
		private const string KEY_CODE_ALPHA_2 = "code-alpha-2";
		private const string KEY_CODE_ALPHA_3 = "code-alpha-3";
	
		public string Id
		{
			get;
			set;
		}
		
		public string CodeAlpha2
		{
			get;
			set;
		}

		public string CodeAlpha3
		{
			get;
			set;
		}

		public string Name;

		
	}



}
