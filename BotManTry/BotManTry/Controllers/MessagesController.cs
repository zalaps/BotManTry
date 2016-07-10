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

        public string Talk { get; set; }


        public MessagesController()
        {
            PartyProfile = new Dictionary<string,string>();
            Greetings = new List<string> { "hi", "hii", "hello", "hey", "man", "orderman", "cygnetorderman"};
            Talk = "Oops! I coudn't understood last message. I'm yet to learn many human conversations :(";
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
                    Talk = "Hey " + activity.From.Name + ", How may I help you!";
                }
              
                Activity reply = activity.CreateReply(Talk);
                await connector.Conversations.ReplyToActivityAsync(reply);
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