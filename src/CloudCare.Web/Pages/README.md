Notes:
- In a web app "states" is data that the application is currently holding in memory, such as user sessions, or in our case the expense when editing a expense or waiting for the OCR to return the expense data.
- THe issue we faced was how do we get the expense data from one page to another. Like from viewing a expense to editing 
- So the way we solved it is that we used a "Shared Box" Service (expenseStateService) which is a state container 
- This is a shared box that is accisible from anywhere in the app
  - This is a singleton service that holds the expense data in memory
  - When we press edit button, or when the OCR process finishes we take the expense and put it into our shared box
  - and then we navigate to the edit page and then use the shared service to take that expense out 
    - So when the addExpense.razor page loads it checks the shared box for an expense
      - if it finds something it takes it out and prefills the form with that data. 