
# Bupa Australia Coding Test - Engineering Excellence Edition

Thank you for your interest in joining the Engineering Excellence team at Bupa. To better assess your strengths and knowledge, please kindly complete the following coding test to the best of your ability, using whichever language, tools, libraries and methods you wish as long as it accomplishes a solution to the problem.

We are considering your skillset and aptitude based on your understanding of the problem and how you go about solving it; we want you to show us how you would tackle this problem if you were already working together as a team at Bupa to release an enterprise-standard product.

# The Issue

The JSON data for this task is provided through an API. More info here with the Swagger Documentation

We have some data on books and their owners. The data of the JSON format is structured to look something like this:

    Book Owner 1
        Name
        Age
        Books
            Book A
                Name
                Type

    Book Owner 2
        Name
        Age
        Books
            Book B
                Name
                Type
            Book C
                Name
                Type

We would like you to build an app that would fetch the API data and output a list of all the books in alphabetical order under a heading of the age category of their owner. Owners aged 18 and above are considered Adults, Owners aged 17 and below are considered Children.

The app you need to build consumes the API provided and outputs a list of all the books in alphabetical order under a heading of the age category of their owner. Owners aged 18 and above are considered Adults, Owners aged 17 and below are considered Children. The app can render to a UI, or can just expose an new API endpoint that returns the data in the format you choose, or both.

## Take Care!

We are the Engineering Excellence team - the part of Bupa that leads the way in modern software development practices - we will be providing designs, platforms and support to the teams that build Bupa's software products; with this in mind, we recommend that you put consideration into the following elements:

* We value automation, consistency and repeatability in your application
* Think about how you can make your application easy to deploy and maintain
* Consider how you would test your application, and which types of tests are interesting for your application design
* Think about any security considerations you might need to apply

## Important information:

* When you select “Get Books” or call the "GetBooks" API, all books are shown, grouped and sorted
* When you provide a “Hardcover only” filter, then only hardcover books should be shown
* Please document any assumptions you make
* The code you present should be of production quality so think about all the code needed before proceeding to production that you normally write
* Submissions will only be accepted via github or a publicly accessible repository
* Use industry best practices
* Use the code to showcase your skill, and have some fun with your solution!

> Once you have completed the test, please submit your solution to your recruitment contact. And again, thank you for your interest in Bupa and hope to meet you soon for an interview.
