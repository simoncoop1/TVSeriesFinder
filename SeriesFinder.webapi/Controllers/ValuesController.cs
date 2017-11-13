using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Xml.XPath;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace SeriesFinder.webapi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
		
		//dictionary connecting channel checkbox names to indexs in Channel array
		public readonly Dictionary<int,int> channelCheckbox = new Dictionary<int,int>(){
									   				  {1,0},
													  {2,1},
													  {3,2},
													  {4,3},
													  {5,15},
													  {6,5},
													  {7,7}													  
													  };
		
		//public readonly Dictionary<int,int> channelCheckbox = new Dictionary<int,int>(){
		//							   				  {7,0}};
		
		//for test
		//public readonly string[] BEGIN = new string [] { "0" };
		//public readonly string[] CHANNEL = new string [] { "BDS||BBC Two" ,"BDS||BBC Four", "BDSHD||Sky Atlantic" };			
		
		//<option value="BDS||LONDON LIVE">LONDON LIVE</option><option value="BDS||ITV2">ITV2</option><option value="BDS||ITV3">ITV3</option><option value="BDS||ITV4">ITV4</option><option value="BDS||Sky Living +1">Sky Living +1</option><option value="BDSHD||ITV Encore HD">ITV Encore HD</option><option value="BDS||FOX">FOX</option><option value="BDS||Pick TV">Pick TV</option><option value="BDS||MTV HD">MTV HD</option><option value="BDS||Comedy Central Extra">Comedy Central Extra</option><option value="BDS||Sky 2">Sky 2</option><option value="BDS||ITV +1 London">ITV +1 London</option><option value="BDS||Alibi">Alibi</option><option value="BDS||Channel 4 +1">Channel 4 +1</option><option value="BDS||E4 PLUS 1">E4 PLUS 1</option><option value="BDS||More4">More4</option><option value="BDS||4seven">4seven</option><option value="BDSHD||BBC Two HD">BBC Two HD</option><option value="BDS||Quest">Quest</option><option value="BDS||Challenge">Challenge</option><option value="BDS||CBS Reality">CBS Reality</option><option value="BDS||CBS action">CBS action</option><option value="BDS||CBS drama">CBS drama</option><option value="BDS||E! Entertainment">E! Entertainment</option><option value="BDSENH||TLC +1">TLC +1</option><option value="BDS||really">really</option><option value="BDS||Lifetime">Lifetime</option><option value="BDS||Sony Channel">Sony Channel</option><option value="BDS||Drama">Drama</option><option value="BDS||Spike">Spike</option><option value="BDS||DMAX">DMAX</option><option value="BDS||Real Lives">Real Lives</option><option value="BDS||Real Lives+1">Real Lives+1</option><option value="BDS||5 USA">5 USA</option><option value="BDS||Channel 5+24">Channel 5+24</option><option value="BDS||5 Star">5 Star</option><option value="BDS||Channel 5 +1">Channel 5 +1</option><option value="BDS||ITV HD London">ITV HD London</option><option value="BDS||ITVBe">ITVBe</option><option value="BDS||ITV2 +1">ITV2 +1</option><option value="BDS||True Entertainment">True Entertainment</option><option value="BDS||FOX HD">FOX HD</option><option value="BDS||ITV3+1">ITV3+1</option><option value="BDS||Quest +1">Quest +1</option><option value="BDS||Home">Home</option><option value="BDS||TruTV">TruTV</option><option value="BDS||E4">E4</option><option value="BDS||ITV4+1 Sky">ITV4+1 Sky</option><option value="BDS||Dave ja vu">Dave ja vu</option><option value="BDS||Sky 1">Sky 1</option><option value="BDS||MTV">MTV</option><option value="BDS||Channel 4 HD">Channel 4 HD</option><option value="BDSENH||DMAX +1">DMAX +1</option><option value="BDSENH||TLC HD">TLC HD</option><option value="BDS||Home and Health">Home and Health</option><option value="BDSENH||Discovery Home and Health +1">Discovery Home and Health +1</option><option value="BDS||Food Network">Food Network</option><option value="BDS||Travel Channel">Travel Channel</option><option value="BDS||Good Food">Good Food</option><option value="BDSHD||ITV Encore">ITV Encore</option><option value="BDS||Sky Movies Premiere">Sky Movies Premiere</option><option value="BDS||Sky Movies Premiere +1">Sky Movies Premiere +1</option><option value="BDSHD||Sky Movies Showcase HD">Sky Movies Showcase HD</option><option value="BDS||Sky Movies Family">Sky Movies Family</option><option value="BDSHD||Sky Movies Comedy HD">Sky Movies Comedy HD</option><option value="BDSHD||Sky Movies Crime &amp; Thriller HD">Sky Movies Crime &amp; Thriller HD</option><option value="BDSHD||Sky Movies Drama &amp; Romance HD">Sky Movies Drama &amp; Romance HD</option><option value="BDS||Film4">Film4</option><option value="BDS||TCM">TCM</option><option value="BDS||TCM 2">TCM 2</option><option value="BDS||horror channel">horror channel</option><option value="BDS||True Movies">True Movies</option><option value="BDS||Movies4Men">Movies4Men</option><option value="BDS||Movies 24">Movies 24</option><option value="BDS||Sky Movies Showcase">Sky Movies Showcase</option><option value="BDS||Sky Movies Greats">Sky Movies Greats</option><option value="BDS||Sky Movies Disney">Sky Movies Disney</option><option value="BDSHD||Sky Movies Action &amp; Adventure">Sky Movies Action &amp; Adventure</option><option value="BDS||Sky Movies Comedy">Sky Movies Comedy</option><option value="BDS||Sky Movies Crime &amp; Thriller">Sky Movies Crime &amp; Thriller</option><option value="BDS||Sky Movies Drama &amp; Romance">Sky Movies Drama &amp; Romance</option><option value="BDSHD||Sky Movies Sci-Fi &amp; Horror">Sky Movies Sci-Fi &amp; Horror</option><option value="BDS||Sky Movies Select">Sky Movies Select</option><option value="BDS||MTV Music">MTV Music</option><option value="BDS||MTV Base">MTV Base</option><option value="BDS||MTV Hits">MTV Hits</option><option value="BDS||VIVA">VIVA</option><option value="BDS||MTV Dance">MTV Dance</option><option value="BDS||MTV Rocks">MTV Rocks</option><option value="BDS||MTV CLASSIC">MTV CLASSIC</option><option value="BDS||VH1">VH1</option><option value="BDS||The Box">The Box</option><option value="BDS||Box Hits">Box Hits</option><option value="BDS||4Music">4Music</option><option value="BDS||Kiss">Kiss</option><option value="BDS||Magic (Emap TV)">Magic </option><option value="BDS||Scuzz">Scuzz</option><option value="BDS||Kerrang!">Kerrang!</option><option value="BDSHD||Sky Sports 1 HD">Sky Sports 1 HD</option><option value="BDSHD||Sky Sports 2 HD">Sky Sports 2 HD</option><option value="BDS||Sky Sports News HQ">Sky Sports News HQ</option><option value="BDS||Sky Sports 1">Sky Sports 1</option><option value="BDS||Eurosport 1">Eurosport 1</option><option value="BDS||Eurosport 2">Eurosport 2</option><option value="BDSENH||Eurosport HD (Europe)">Eurosport HD </option><option value="BDS||BT Sport 1">BT Sport 1</option><option value="BDS||At The Races">At The Races</option><option value="BDS||BT Sport 2">BT Sport 2</option><option value="BDS||MUTV">MUTV</option><option value="BDS||Chelsea TV">Chelsea TV</option><option value="BDS||BT Sport//ESPN">BT Sport//ESPN</option><option value="BDS||Liverpool FC TV">Liverpool FC TV</option><option value="BDS||Racing UK">Racing UK</option><option value="BDS||Sky Sports 2">Sky Sports 2</option><option value="BDS||Sky Sports 3">Sky Sports 3</option><option value="BDS||Sky Sports 4">Sky Sports 4</option><option value="BDSENH||Eurosport 2 HD">Eurosport 2 HD</option><option value="BDS||Motors TV">Motors TV</option><option value="BDSHD||BT Sport//ESPN HD">BT Sport//ESPN HD</option><option value="BDS||Bloomberg">Bloomberg</option><option value="BDS||BBC News">BBC News</option><option value="BDS||BBC PARLIAMENT">BBC PARLIAMENT</option><option value="BDS||CNBC">CNBC</option><option value="BDS||CNN">CNN</option><option value="BDS||EuroNews">EuroNews</option><option value="BDS||NDTV 24x7">NDTV 24x7</option><option value="BDS||Al Jazeera International">Al Jazeera International</option><option value="BDS||Sky News">Sky News</option><option value="BDS||Discovery">Discovery</option><option value="BDSENH||Discovery Channel +1">Discovery Channel +1</option><option value="BDS||ID">ID</option><option value="BDS||Animal Planet">Animal Planet</option><option value="BDS||Discovery Turbo">Discovery Turbo</option><option value="BDS||Discovery Science">Discovery Science</option><option value="BDS||National Geographic HD">National Geographic HD</option><option value="BDSHD||Nat Geo Wild HD">Nat Geo Wild HD</option><option value="BDSHD||History HD">History HD</option><option value="BDS||H2">H2</option><option value="BDS||PBS America">PBS America</option><option value="BDS||Discovery History">Discovery History</option><option value="BDSENH||Discovery History +1">Discovery History +1</option><option value="BDS||YESTERDAY">YESTERDAY</option><option value="BDS||Community Channel Full">Community Channel Full</option><option value="BDS||Nat Geo">Nat Geo</option><option value="BDS||Nat Geo Wild">Nat Geo Wild</option><option value="BDS||History">History</option><option value="BDSENH||Animal Planet UK +1">Animal Planet UK +1</option><option value="BDSENH||Investigation Discovery +1">Investigation Discovery +1</option><option value="BDSHD||Crime+Inv HD">Crime+Inv HD</option><option value="BDS||Crime+Inv">Crime+Inv</option><option value="BDSENH||Discovery Science +1">Discovery Science +1</option><option value="BDSENH||Animal Planet HD">Animal Planet HD</option><option value="BDS||Eden">Eden</option><option value="BDS||God Channel">God Channel</option><option value="BDS||Boomerang">Boomerang</option><option value="BDS||Nicktoons">Nicktoons</option><option value="BDS||Disney Junior">Disney Junior</option><option value="BDS||CBBC">CBBC</option><option value="BDS||Cbeebies">Cbeebies</option><option value="BDS||Nick Jr.">Nick Jr.</option><option value="BDS||Nick Jr. Too">Nick Jr. Too</option><option value="BDS||CITV">CITV</option><option value="BDS||Disney XD">Disney XD</option><option value="BDS||Baby TV">Baby TV</option><option value="BDS||Disney  Channel">Disney  Channel</option><option value="BDS||Nickelodeon">Nickelodeon</option><option value="BDS||Cartoon Network">Cartoon Network</option><option value="BDS||QVC">QVC</option><option value="BDS||Ideal World">Ideal World</option><option value="BDS||Create and Craft">Create and Craft</option><option value="BDS||B4U Movies">B4U Movies</option><option value="BDS||Star Plus">Star Plus</option><option value="BDS||Zee TV">Zee TV</option><option value="BDS||ZEE Cinema">ZEE Cinema</option><option value="BDS||TV5 Europe">TV5 Europe</option></select>
		
		//public string xhtml;
		
		public static string controllerValue = "no";
		
		public static int countT = 0;
		
		
		// GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
			//await GenerateProgramListingPages();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
		//	return "value" + id;
        //}
			/*
			Product product = new Product();
			product.Name = "Apple";
			product.Expiry = new DateTime(2008, 12, 28);
			product.Sizes = new string[] { "Small" };

			string json = JsonConvert.SerializeObject(product);		
			*/
		
		// GET api/values/5
		[HttpGet("{id}")]
        public string Get(int id){
			//JArray array = new JArray();
			//array.Add("Manual text");
			//array.Add(new DateTime(2000, 5, 23));

			//JObject o = new JObject();
			//o["MyArray"] = array;
			
			//string json = o.ToString();
			//return json;
			//return "value" + id;
			
			//await GenerateProgramListingPages();
			
			//CHANNEL[channelCheckbox[id]]
			
			Console.WriteLine(id);
			
			Tuple<int,DateTime> myTuple = new Tuple<int,DateTime>(channelCheckbox[id],DateTime.Now);

			if(WebCollector.Data.ContainsKey(myTuple)){
				return WebCollector.Data[myTuple].ToString();
			}
			else{
				return "Key Not Found";
				//return placeholder error json here
			}
			
			//test json
			/*
			return @"[
						{
									""title"" : ""I Know Who You Are"",
									""subtitle"" : ""Series 2, Episode 1"",
									""timing"" : ""Saturday 4th November on BBC Four from 9:00pm to 10:15pm""						
						}
					 ]";
			*/
						
        }

		[HttpGet("Scopey")]
		public string GetAllScopey(){
			String retVal = ValuesController.controllerValue;
			ValuesController.controllerValue = "yes";
			
			//static members can be accessible during the life of the app
			//if(ValuesController.running == null){
			//	ValuesController.running = new Schedule();
			//	RunUpdate();
			//}
			
			
			return retVal+controllerValue+"#"+ ValuesController.countT +"#";
		}
		
		public String ChangeUrl(String url){
			return null;
		}
		
		        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
	
	/*
		In memstore of results
	*/
		
	/*
	public class CompareDictionaryKey : IEqualityComparer<Tuple<int,DateTime>>{
	
		public bool Equals (Tuple<int,DateTime> x, Tuple<int,DateTime> y){ return true;}
		public int GetHashCode (Tuple<int,DateTime> T){
			return 1;
		}
	*/

}

namespace SeriesFinder.webapi
{
	 public class Schedule{
	
		public static void RunUpdate(){

			Thread t = new Thread(new ThreadStart(delegate
			{
			   while(true){
					WebCollector.StartAll();
			   
					//Controllers.ValuesController.countT++;
					//System.Threading.Thread.Sleep(10000);
					
					//sleep untill the next day
					DateTime dt = DateTime.Now;
					DateTime dt2 = dt.AddDays(1);
					DateTime dt3 = new DateTime(dt2.Year,dt2.Month,dt2.Day,0,0,0,dt2.Kind);
					TimeSpan ts = dt3.ToUniversalTime() - dt.ToUniversalTime(); //convert to utc 
					//because arithmetic to not include daylight savings but conversions between timezone and utc does.,
					TimeSpan ts2 = dt3 - dt;
					Console.WriteLine("{0:f} WaitThisLong:{1} minutes",dt,ts.TotalMinutes);
					//System.Threading.Thread.Sleep(ts2);
					System.Threading.Thread.Sleep((int)ts.TotalMilliseconds);

				}
			}));
			t.IsBackground = true;
			t.Start();

			//while(true){
			//	countT++;
			//	Console.WriteLine(countT);
			//	System.Threading.Thread.Sleep(10000);
				
			//}
		}
	
	}
	
	public class WebCollector{
	
		private static HttpClient _HttpClient = new HttpClient();
	
		public static readonly string URL_BASE = "https://accessibility.sky.com/tvguide/";
		public static readonly string URL_TEXT = "https://accessibility.sky.com/tvguide/text/";
		public static readonly string REMOVE_FROM_PROGRAM_URL = "../";
		public static readonly string NAMESPACE_REMOVE_STR = " xmlns=\"http://www.w3.org/1999/xhtml\" ";
		public static readonly string TV_LST_QUERY = "/html/body/div/div/ul/li/a[@class=\"rbm_title\"]";
		public static readonly string[] BEGIN = new string [] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
	
		//the channel names used on the tvguide website
		public static readonly string[] CHANNEL = new string [] { "BDS||BBC One London", "BDS||BBC Two", "BDS||ITV", "BDS||Channel 4", "BDS||Channel 5", "BDS||Sky 1 HD", "BDS||Sky Living", "BDSHD||Sky Atlantic", "BDS||W", "BDS||GOLD", "BDS||Dave", "BDS||Comedy Central", "BDS||UNIVERSAL", "BDS||SyFy", "BDSHD||BBC One HD", "BDS||BBC Four", "BDS||Sky Arts"};
		//public static readonly string[] CHANNEL = new string [] { "BDS||BBC Four"};
		
		public static readonly String URL_CHANNEL_PG_TEMPLATE = "https://accessibility.sky.com/tvguide/text/?colour=one&tvgRegion=London&tvgFlagFilter=%25&tvgBegin=&tvgChannel=";
		public static readonly String B_SRCH = "&tvgBegin=";
		public static  readonly String C_SRCH = "&tvgChannel=";		
		
		//in memory store
		public static Dictionary<Tuple<int,DateTime>,JArray> Data = new Dictionary<Tuple<int,DateTime>,JArray>(new CompareDictionaryKey());
		
		//get http client we can reuse the same one
		//private static HttpClient GetHttpClient(){
		//	if(_HttpClient == null)
		//		_HttpClient = new HttpClient();
		//	return _HttpClient;
		//}		
	
		public static async void StartAll(){
	 
			for (int i=0;i<CHANNEL.Length;i++){
			
				JArray array = new JArray();
			
				foreach(String url in ChannelPages(new String[]{CHANNEL[i]})){

					//load a channel page
					String channelPage = await HttpRequest(url);

					Console.WriteLine(1);

					//get the program url
					foreach(String ProgramUrl in GenerateProgramPages(channelPage)){
						JObject o = ReadProgramPage(await HttpRequest(ProgramUrl));
						if(o !=null)
							array.Add(o);
					}
				}
				
				Data.Add(new Tuple<int,DateTime>(i,DateTime.Now),array);

			}
			Console.WriteLine("end");
			
			//return array.ToString();

		}

		public static async Task<String> HttpRequest(String url){
			
			// Create a New HttpClient object.
			//HttpClient client = GetHttpClient();
			HttpClient client = new HttpClient();

			// Call asynchronous network methods in a try/catch block to handle exceptions
			try	
			{
			   HttpResponseMessage response = await client.GetAsync(url);
			   response.EnsureSuccessStatusCode();
			   string responseBody = await response.Content.ReadAsStringAsync();
			   // Above three lines can be replaced with new helper method below
			   // string responseBody = await client.GetStringAsync(uri);

			   //Console.WriteLine(responseBody);
			   
			   //ReadXHTML(responseBody);
			   
			   return responseBody;
			}  
			catch(HttpRequestException e)
			{
			   Console.WriteLine("\nException Caught!");	
			   Console.WriteLine("Message :{0} ",e.Message);
			   
			   
			   throw e;
			   return "Error";
			   
			}
			finally{
				//Console.WriteLine("this is executing");
				
				// Need to call dispose on the HttpClient object
				// when done using it, so the app doesn't leak resources
				client.Dispose();
			}
		
		}
		
		public static async Task<String> HttpRequest(){
			return await HttpRequest(URL_TEXT);
		}
		
		public static void ReadXHTML(String text){
			
			//remove namespace
			text = RemoveNameSpace(text);
		
			XPathDocument xPathDoc = new XPathDocument(new StringReader(text));
			XPathNavigator nav = xPathDoc.CreateNavigator();
			
			//test remove namespace
			//string text2 = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>   <bookstore lang=\"en\" xml:lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" >   <book genre=\"autobiography\" publicationdate=\"1981-03-22\" ISBN=\"1-861003-11-0\">   <title>The Autobiography of Benjamin Franklin</title>  <author>  <first-name>Benjamin</first-name>   <last-name>Franklin</last-name>    </author>  <price>8.99</price>  </book> <book genre=\"novel\" publicationdate=\"1967-11-17\" ISBN=\"0-201-63361-2\"> <title>The Confidence Man</title> <author><first-name>Herman</first-name><last-name>Melville</last-name></author><price>11.99</price></book><book genre=\"philosophy\" publicationdate=\"1991-02-15\" ISBN=\"1-861001-57-6\">        <title>The Gorgias</title>          <author>              <name>Plato</name>          </author>          <price>9.99</price>      </book></bookstore>";
			//RemoveNameSpace(text);
			//Console.WriteLine("---------- no namespace: " + RemoveNameSpace(text2));
			
			XPathDocument document = new XPathDocument(new StringReader(text));
			XPathNavigator navigator = document.CreateNavigator();

			//to use with name space use this code to setup a prefix
			//then use XmlNamespaceManager as querry param
			// for simplicity I removed namespace definition from the input xml
			// namespaces are useful for multiple xml documents to avaoild name clashes
			//var theNamespaceIndicator = new XmlNamespaceManager( new NameTable() );
			//theNamespaceIndicator.AddNamespace( "thex", "http://www.w3.org/1999/xhtml" );
			//XPathNodeIterator nodes = navigator.Select("/thex:bookstore",theNamespaceIndicator);
			
			//you can use localname to sidestep namespaces
			//XPathNodeIterator nodes = navigator.Select("*[local-name()='bookstore']/*[local-name()='book']");
			
			//select the titles
			XPathNodeIterator nodes = navigator.Select(TV_LST_QUERY);
			
			//alternate xpath
			//*[@id="rbm_channelWrapper"]/ul/li/a
			
			Console.WriteLine("---------- nodes count " + nodes.Count + "-----");

			while (nodes.MoveNext()){
				Console.WriteLine(nodes.Current.Value);
				String href = nodes.Current.GetAttribute("href", "");
				Console.WriteLine(href);
			}
			
			//String xpathExpr = "/html/body/div/div/ul/li";
			//String xpathExpr = "/html/body/div/div";
			//XPathNodeIterator nodeIter = nav.Select(xpathExpr);
			//nodeIter.MoveNext();
			//XPathNavigator nodesNv2 = nodeIter.Current;
			//XPathNodeIterator nodesText = nodesNv2.SelectDescendants(XPathNodeType.Text, false);
			//while (nodesText.MoveNext())
			//	Console.WriteLine(nodesText.Current.Value);
				
		
			//Console.WriteLine("expression result {0}",nodesNv2.Value );
			
		}
		
		/*
			avoiding complexity of namespaces
		*/
		public static String RemoveNameSpace(String xmlNameSpace){
			
			int ind = xmlNameSpace.IndexOf(NAMESPACE_REMOVE_STR);
			int length = NAMESPACE_REMOVE_STR.Length;
			
			return xmlNameSpace.Substring(0,ind) + xmlNameSpace.Substring(ind+length);
		}
		
		public static async Task<List<String>> GenerateProgramListingPages(){
			//URL_TEXT
			//BEGIN
			//CHANNEL			
			
			foreach (String url in ChannelPages()){
							
				//load a channel page
				String channelPage = await HttpRequest(url);
				
				//get the program url
				foreach(String ProgramUrl in GenerateProgramPages(channelPage)){
					ReadProgramPage(await HttpRequest(ProgramUrl));
				}					
				
				//foreach program url load it and return string
				
				
				//identify episode number and print to console if off interest
			}
			
			//Select(x => x * x);

			//Console.WriteLine(num);
			
			//https://accessibility.sky.com/tvguide/text/?colour=one&tvgRegion=London&tvgFlagFilter=%25&tvgBegin=0&tvgChannel=BDSHD%7C%7CBBC+One+West+Midlands
			
			URL_CHANNEL_PG_TEMPLATE.Select(x => x);
			
			return null;
		}
		
		public static JObject ReadProgramPage(String ProgramPage){
					
					
					//Console.WriteLine("-----------------");
					//Console.WriteLine(ProgramUrl);
					//Console.WriteLine("-----------------");
					
					//"//*[@id="rbm_details"]/p[1]"
					//"//*[@id="rbm_details"]/p[@class=\"rbm_title\"]"
					
					ProgramPage = RemoveNameSpace(ProgramPage);
					
					XPathDocument xPathDoc = new XPathDocument(new StringReader(ProgramPage));
					XPathNavigator nav = xPathDoc.CreateNavigator();					
					
					//XPathNavigator node1 = nav.SelectSingleNode("//*[@class=\"rbm_subtitle \"]");	
					//XPathNavigator node2 = nav.SelectSingleNode("//*[@class=\"rbm_title\"]");				
					string subtitle = (string)nav.Evaluate("string(//*[@class=\"rbm_subtitle \"])");					
					string title = (string)nav.Evaluate("string(//*[@class=\"rbm_title\"])");
					string timing = (string)nav.Evaluate("string(//*[@class=\"rbm_timing\"])");
					//<p class="rbm_timing">Today on BBC One London from 11:55pm to 12:25am</p>
					
					//Regex rx = new Regex(@"\bseason\s*\d+\b",RegexOptions.Compiled | RegexOptions.IgnoreCase);
					// Define a test string.        
					//string text = "The the quick brown fox  fox Season001 jumps over the lazy dog dog.";
					// Find matches.
					//MatchCollection matches = rx.Matches(text);
					
					
					//if(node1 != null && node2 != null &&
					//   Regex.IsMatch(node1.Value, @"\bepisode\s*0*1\b", RegexOptions.Compiled | RegexOptions.IgnoreCase)){					

						//Console.WriteLine("######################" + node1.Value + ",	" + node2.Value);
						//Console.WriteLine("######################" + node3  );
						
					//}
					
					if(Regex.IsMatch(subtitle, @"\bepisode\s*0*1\b", RegexOptions.Compiled | RegexOptions.IgnoreCase)){
						Console.WriteLine("##" + title + " " + subtitle + " " + timing);
						
						//json object;
						JObject o = new JObject();
						o["title"] = title;
						o["subtitle"] = subtitle;
						o["timing"] = timing;
						
						return o;
						
					}
					
					return null;
		}
		
		public static IEnumerable<String> ChannelPages(){
			return ChannelPages(CHANNEL);
		}
		
		public static IEnumerable<String> ChannelPages(String[] channels){
			foreach(String day in BEGIN){
				foreach(String ch in channels){
						String UrlPg = URL_CHANNEL_PG_TEMPLATE.Insert(URL_CHANNEL_PG_TEMPLATE.IndexOf(B_SRCH)+B_SRCH.Length,day);						
						yield return UrlPg.Insert(UrlPg.IndexOf(C_SRCH)+C_SRCH.Length,ch);
					}
			}
		}

		/*
			stuff
		*/
		public static IEnumerable<String> GenerateProgramPages(String ChannelPage){
		
			ChannelPage = RemoveNameSpace(ChannelPage);

			XPathDocument xPathDoc = new XPathDocument(new StringReader(ChannelPage));
			XPathNavigator nav = xPathDoc.CreateNavigator();
			
			XPathDocument document = new XPathDocument(new StringReader(ChannelPage));
			XPathNavigator navigator = document.CreateNavigator();
			
			//select the titles
			XPathNodeIterator nodes = navigator.Select(TV_LST_QUERY);
			
			 
			/*
			String[] days = {"Monday", "Wednesday", "Friday"};
			
			foreach(String s in days)
			{
				
			}
			*/
			
			
			//alternate xpath
			//*[@id="rbm_channelWrapper"]/ul/li/a
			
			//Console.WriteLine("---------- nodes count " + nodes.Count + "-----");

			while (nodes.MoveNext()){
				//Console.WriteLine(nodes.Current.Value);
				String href = nodes.Current.GetAttribute("href", "");
				yield return URL_BASE + href.Remove(href.IndexOf(REMOVE_FROM_PROGRAM_URL),REMOVE_FROM_PROGRAM_URL.Length);
				//Console.WriteLine(href);
			}
			
		}		
	
	}
	
	public class CompareDictionaryKey : EqualityComparer<Tuple<int,DateTime>>{
	
		public override bool Equals (Tuple<int,DateTime> x, Tuple<int,DateTime> y){ 
			if(x.Item1 == y.Item1 && x.Item2.Day == y.Item2.Day && x.Item2.Month == y.Item2.Month && x.Item2.Year == y.Item2.Year){
				//var culture = new CultureInfo("en-GB");
				//localDate.ToString(culture)
				return true;
			}
			return false;
		}	
		
		public override int GetHashCode (Tuple<int,DateTime> t){
			return t.Item1 ^ t.Item2.Day ^ t.Item2.Month ^ t.Item2.Year;
		}
	}	
}
