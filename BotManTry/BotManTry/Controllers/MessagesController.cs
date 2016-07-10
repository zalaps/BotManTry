using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Collections.Generic;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace BotManTry
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public Dictionary<string,string> PartyProfile { get; set; }

        public List<string> Greetings { get; set; }

        public List<string> HindiGreetings { get; set; }

        public List<string> HindiGreetings1 { get; set; }

        public List<string> OrderDetails { get; set; }

        public List<string> Talk { get; set; }


        public MessagesController()
        {
            PartyProfile = new Dictionary<string,string>();
            Greetings = new List<string> { "hi", "hii", "hello", "hey", "man", "orderman", "cygnetorderman", "hi there", "hello there"};
            OrderDetails = new List<string>();
            HindiGreetings = new List<string> { "kya hal hai?"};
            HindiGreetings1 = new List<string> { "Aur kya chal raha hai?" };
            Talk = new List<string>();
            Talk.Add("Oops! I coudn't understood last message. I'm yet to learn many human conversations :(");
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                
                // Chat message text comes in activity.text
                var message = activity.Text.Trim(' ', '.', '!', ',');

                if(Greetings.Contains(message, StringComparer.OrdinalIgnoreCase))
                {
                    Talk.Clear();
                    Talk.Add("Hey " + activity.From.Name + "! As I see, You are propriter of Allstar Footwears and an existing customer of Paragon! \n" 
                        + "For your ready refernce latest catalog of Paragon product offerings is located at https://cygnetorderman.azurewebsites.net/content/Paragon_10072016_013910.pdf. \n" 
                        + "What would you like to order today? Let me suggest a way to start. To order 4 cartons of model 3087 with black colour, you may say: 3087-black = 4");
                }
                else if (message.Contains("="))
                {
                    OrderDetails.Add(message);
                    Talk.Clear();
                    Talk.Add("Copy that roger. Let me know once done. I would present an order summary for your approval.");
                }
                else if(HindiGreetings.Contains(message, StringComparer.OrdinalIgnoreCase))
                {
                    Talk.Add("Mast hai bhai! Aur sunao! :)");
                }
                else if (HindiGreetings1.Contains(message, StringComparer.OrdinalIgnoreCase))
                {
                    Talk.Add("Yahan to FOGG chal raha hai!!! :P");
                }
                else if (message.Contains("done"))
                {
                    var s = "Great! Here is a snap shot from what we have prepared till now. \n";
                    foreach (var orderDetail in OrderDetails)
                    {
                        s = s + orderDetail + " \n";
                    }
                    s = s + "Do confirm if everything is OK.";
                    Talk.Clear();
                    Talk.Add(s);
                }


                /* Send all replys */
                foreach (var talk in Talk)
                {
                    Activity reply = activity.CreateReply(talk);
                    await connector.Conversations.ReplyToActivityAsync(reply);              
                }               
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}