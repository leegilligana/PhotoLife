# PhotoLife: 2023/24 CS Comps

## Description
[Official Project Outline](https://www.cs.carleton.edu/cs_comps/2324/photoLife/index.php)
>Welcome to the PhotoLife Comps! Our team embarked on this project to address ethical concerns surrounding the use of
image metadata and to exemplify what could be done with the personal data stored in the images we take, share and utilize
everyday. We developed several algorithms capable of extracting, analyzing and categorizing the metadata from each group
of photos uploaded to our site.<br /><br />
Well, how do these algorithms work with the photos and with each other? First, we run a 
specific group of photos provided by the user through our Metadata Extractor, which extracts the metadata from each photo
and stores them in our database (the physical photos are not stored). To upload photos to our extractor, users are required
to sign in with their (Carleton) email and provide access to a folder filled with the photos they want our algorithms to 
have access to and work with. From there, the user has the option to generate albums from those photos using our Album 
Generator, which groups photos based on specific filters chosen by the user and places them in labeled folders in their Drive. 
Or, the user can ask for a photo profile, where our Profile Maker analyzes the photo data and displays stats (predictions) 
about the group of photos. After the profile maker or album generator algorithms have been called, the user's metadata is 
deleted from our database. And the process repeats if the user wants to interact again later.

## Workflows
***Metadata Extractor***<br />
The user signs into their Google account and gives our site permissions -> the metadata extractor API retrieves all of the (new) photo metadata
from the photos in their "PhotoLife" folder and saves them to the database with a unique identifier
<br /><br />
***Album Generator***<br />
The user choose at least one filter that they want their group of folders to be sorted by for the new album(s) -> that request gets sent to the Album
Generator API (which is merged with the Metadata Extractor API) -> the Album Generator API calls the database and retrieves all the photos that match the filter(s) chosen
-> that list gets sent back to the API, which formats the data and sends it to the user's Google Drive -> in the Google Drive, a new "output" folder with the time and date
it was created is created with a copy of all of the photos from the matching list that the API sent -> after all of the photos are uploaded, the user can retrieve their newly
created album from their Google Drive
<br /><br />
***Profile Maker***<br />
The user chooses which personal facts they want to be generated from their group of photos -> those options get sent to the Profile Maker API and the profile
maker calls on the database to get the user's metadata -> that information is sent back the API, which then calculates the corresponding profile attributes of the user (like
the average time their photos are taken) -> those results are sent to the front-end UI and displayed to the user.

## Requirements (Tools)
- Visual Studio
- .NET
- Google API

## How To Run
1. Run the metadata extractor API so that its endpoints can be called from the front-end
2. Run the PhotoLife project to display the interfact that calls each API

## Features
1. A Metadata Extractor that stripts user-uploaded photos of their (most relevant) metadata and stores that info in a database with the user's unique ID
2. An Album Generator that gathers user-chosen filters to generate new albums filled with uploaded photos that match the criteria chosen
3. A Profile Maker that generates facts about user's photo taking habits based on the photos they upload and the information they want to learn about their photos
4. Postgres Database (temporarily) holding user image metadata
5. Google API Service that handles user sign-ins, manages photo metadata that is uploaded to the user's cloud, and calls both the album generator API and the profile maker API to generate results

## Project Status
Our project works as intended and we have several different components that communicate each other. It has general error checking, especially when it comes to running either the album generator or profile maker with a google account that has accessible photos. We included a delete all option on our sign-in page that allows users to remove any metadata they uploaded from our databases. We are limited to the number of requests we can make and the amount of metadata we can store, but that has not been a problem yet given it's not a frequently used platform (yet?).
<br /><br />
Moving forward, we would commit ourselves to more robust testing, an expansion of our Album Generator and Profile Maker, and some other features which include (but not limited to): Face Recognition, Who/What is in each photo, Coordinates to a Visual Map that the user can interact with, A Heat Map of Visited Locations, User Behavior/Routines in Profile Maker, and the integration of online Photo Libraries.

## Credits
This project was created and maintained by: Sunny Kim, Peyton Bass, Alejandro Gonzalez, Aidan Lee-Gilligan, and T'airra Champliss. It was advised by Amy Csizmar-Dalal.

## License
All Rights Reserved to the Carleton College PhotoLife 2023/2024 Comps Group.
