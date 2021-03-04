// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.11.1
using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema.Teams; //add
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaskModuleNew.Bots
{
    public class EchoBot : TeamsActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Text.ToLower() == "hi" || turnContext.Activity.Text.ToLower() == "hello")
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(GetAdaptiveCard()), cancellationToken);
            }
            else if (turnContext.Activity.Text.ToLower() == "sign in" || turnContext.Activity.Text.ToLower() == "login")
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(GetLoginAdaptiveCard()), cancellationToken);
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
        protected override async Task<TaskModuleResponse> OnTeamsTaskModuleFetchAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(taskModuleRequest);
            var obj = JObject.Parse(json);
            var name = (string)obj["data"]["name"];
            var choice = (string)obj["data"]["data"];
            var mail = (string)obj["data"]["mail"];
            if (choice == "login")
            {
                return new TaskModuleResponse
                {
                    Task = new TaskModuleContinueResponse
                    {
                        Value = new TaskModuleTaskInfo()
                        {
                            Card = CreateThanksCard(mail, name),
                            Height = 200,
                            Width = 400,
                            Title = "Adaptive Card: Inputs",
                        },
                    }
                };
            }
            return new TaskModuleResponse
            {
                Task = new TaskModuleContinueResponse
                {
                    Value = new TaskModuleTaskInfo()
                    {
                        Card = CreateAdaptiveCardAttachment(name),
                        Height = 200,
                        Width = 400,
                        Title = "Adaptive Card: Inputs",
                    },
                }
            };
        }
        protected override async Task<TaskModuleResponse> OnTeamsTaskModuleSubmitAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            return new TaskModuleResponse
            {
                Task = new TaskModuleMessageResponse()
                {
                    Value = "Thanks!",
                },
            };
        }
        private Attachment GetAdaptiveCard()
        {
            AdaptiveCard card = new AdaptiveCard("1.2")
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock
                    {
                        Text="Trigger Card",
                        Size=AdaptiveTextSize.Large,
                        Color=AdaptiveTextColor.Accent,
                        Weight=AdaptiveTextWeight.Bolder,
                        HorizontalAlignment=AdaptiveHorizontalAlignment.Center
                    },
                    new AdaptiveColumnSet
                    {
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text="Name",
                                        Size=AdaptiveTextSize.Large,

                                    }
                                },
                                Width=AdaptiveColumnWidth.Auto
                            },
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextInput
                                    {
                                        Placeholder="enter name",
                                        Id="name"
                                    }
                                }
                            }
                        }
                    },
                    new AdaptiveActionSet
                    {
                        Actions=new List<AdaptiveAction>()
                        {
                            new AdaptiveSubmitAction
                            {
                                Title = "Submit",
                                Style = "positive",
                                Type=AdaptiveSubmitAction.TypeName,
                                Data= new Dictionary<string, object>()
                                {{"msteams",new Dictionary<string,string>(){{"type","task/fetch"},{"value","{\"Id\":\"name\"}"}} },{"data","submit"}}
                            },
                        }
                    }
                }
            };
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        private Attachment CreateAdaptiveCardAttachment(string name)
        {
            AdaptiveCard card = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock
                    {
                        Text="thanks "+name+" for visiting"
                    },
                     new AdaptiveTextBlock
                    {
                        Text="Feedback",
                        Size=AdaptiveTextSize.Medium,
                        Color=AdaptiveTextColor.Accent,
                        Weight=AdaptiveTextWeight.Bolder
                    },
                    new AdaptiveColumnSet
                    {
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextInput
                                    {
                                        Id="input"
                                    }
                                }
                            }
                        }
                    },
                     new AdaptiveActionSet
                     {
                        Actions=new List<AdaptiveAction>()
                        {
                            new AdaptiveSubmitAction
                            {
                                Title = "Submit",
                                Style = "positive",
                                Type=AdaptiveSubmitAction.TypeName,
                                Data= new Dictionary<string, object>()
                                {
                                {
                                "msteams",new Dictionary<string,string>()
                                {
                                {
                                "type","task/Submit"
                                },
                                {
                                "value","{\"Id\":\"input\"}"
                                //"value",json
                                }
                                }
                                },
                                {
                                "data","submit"
                                }
                                }
                            },
                        }
                     }
                },
            };
            Attachment at = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return at;
        }
        private Attachment GetLoginAdaptiveCard()
        {
            AdaptiveCard card = new AdaptiveCard("1.2")
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock
                    {
                        Text="Trigger Card",
                        Size=AdaptiveTextSize.Large,
                        Color=AdaptiveTextColor.Accent,
                        Weight=AdaptiveTextWeight.Bolder,
                        HorizontalAlignment=AdaptiveHorizontalAlignment.Center
                    },
                    new AdaptiveColumnSet
                    {
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text="Name",
                                        Size=AdaptiveTextSize.Large,
                                    }
                                },
                                Width=AdaptiveColumnWidth.Auto
                            },
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextInput
                                    {
                                        Placeholder="enter name",
                                        Id="name"
                                    }

                                }
                            }
                        }
                    },
                    new AdaptiveColumnSet
                    {
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text="Mail",
                                        Size=AdaptiveTextSize.Large,

                                    }
                                },
                                Width=AdaptiveColumnWidth.Auto
                            },
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextInput
                                    {
                                        Placeholder="enter Mail",
                                        Id="mail"
                                    }
                                }
                            }
                        }
                    },
                    new AdaptiveActionSet
                    {
                        Actions=new List<AdaptiveAction>()
                        {
                            new AdaptiveSubmitAction
                            {
                                Title = "Submit",
                                Style = "positive",
                                Type=AdaptiveSubmitAction.TypeName,
                                Data= new Dictionary<string, object>()
                                {
                                {
                                "msteams",new Dictionary<string,string>()
                                {
                                {
                                "type","task/fetch"
                                },
                                {
                                "value","{\"Id\":\"name\"}"
                                //"value",json
                                }
                                }
                                },
                                {
                                "data","login"
                                }
                                }
                            },
                        }
                    }
                }
            };
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            return attachment;
        }
        private Attachment CreateThanksCard(string mail, string name)
        {
            AdaptiveCard card = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock
                    {
                        Text="thanks "+name+ " for registering with "+mail+" "
                    }
                }
            };
            Attachment at = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return at;
        }
    }
}