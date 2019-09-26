// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace Microsoft.BotBuilderSamples
{
    public class MainDialog : ComponentDialog
    {
        protected readonly ILogger _logger;

        public MainDialog(ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _logger = logger;

            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ChoiceCardStepAsync,
                ShowCardStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        // 1. Prompts the user if the user is not in the middle of a dialog.
        // 2. Re-prompts the user when an invalid input is received.
        private async Task<DialogTurnResult> ChoiceCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MainDialog.ChoiceCardStepAsync");

            // Create the PromptOptions which contain the prompt and re-prompt messages.
            // PromptOptions also contains the list of choices available to the user.
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("What Azure Category would you like to see?"),
                RetryPrompt = MessageFactory.Text("That was not a valid choice, please select a card or number from 1 to 4."),
                Choices = GetChoices(),
            };

            // Prompt the user with the configured PromptOptions.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        // Send a Rich Card response to the user based on their choice.
        // This method is only called when a valid prompt response is parsed from the user's response to the ChoicePrompt.
        private async Task<DialogTurnResult> ShowCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MainDialog.ShowCardStepAsync");
            
            // Cards are sent as Attachments in the Bot Framework.
            // So we need to create a list of attachments for the reply activity.
            var attachments = new List<Attachment>();
            
            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);
            
            // Decide which type of card(s) we are going to show the user
            switch (((FoundChoice)stepContext.Result).Value)
            {
                case "Announcements":                   
                    foreach (var item in Cards.GetAnnouncements()) 
                    {
                        reply.Attachments.Add(item.ToAttachment());
                    }
                    break;
                case "Big Data":
                    foreach (var item in Cards.GetBigData())
                    {
                        reply.Attachments.Add(item.ToAttachment());
                    }
                    break;
                case "Cloud Strategy":
                    foreach (var item in Cards.GetCloudStrategy())
                    {
                        reply.Attachments.Add(item.ToAttachment());
                    }
                    break;
                case "Developer":
                    foreach (var item in Cards.GetDeveloper())
                    {
                        reply.Attachments.Add(item.ToAttachment());
                    }
                    break;
                default:
                    // Display a carousel of all the rich card types.                    
                    foreach (var item in Cards.GetAll())
                    {
                        reply.Attachments.Add(item.ToAttachment());
                    }
                    break;
            }

            // Send the card(s) to the user as an attachment to the activity
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            // Give the user instructions about what to do next
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Type anything to see another category."), cancellationToken);

            return await stepContext.EndDialogAsync();
        }

        private IList<Choice> GetChoices()
        {
            var cardOptions = new List<Choice>()
            {

                new Choice() { Value = "Announcements", Synonyms = new List<string>() { "news" } },
                new Choice() { Value = "Big Data", Synonyms = new List<string>() { "data", "big" } },
                new Choice() { Value = "Cloud Strategy", Synonyms = new List<string>() { "cloud", "strategy" } },
                new Choice() { Value = "Developer", Synonyms = new List<string>() { "dev" } }
            };
            return cardOptions;
        }
    }
}
