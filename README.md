# Dummy Participant Generator for MS Teams
 
This is a simple app that uses Chrome and the web version of [Microsoft Teams](https://www.microsoft.com/en-ca/microsoft-teams/join-a-meeting) to add a bunch of dummy participants to a meeting.

The dummy participants will all have the name **Test Participant X**, and will send a dummy camera and microphone.

## Purpose

Have you ever needed to add a bunch of test participants to a meeting? Perhaps you have no friends to call, or you've asked your so-called friends for one too many favors.

I've needed something like this to simulate a bunch of participants on a Teams call when setting up vMix instances. So I made this quick utility.

![Demo image with 10 participants.](/website/demo1.jpg)

*NOTE: this utility is works-on-my-machine certified.* We make no warranties as to whether it'll work on yours.

## How to Use

1. Run `Tractus.TeamsDummyTest.exe`. 
2. Provide the meeting ID, password, and number of dummy participants
3. When finished, type `q` and hit enter to exit the console app.

You can also supply these arguments on the command line.

`.\Tractus.TeamsDummyTest.exe "123 456 789 012" "PASSword" 3`

The above will join the meeting with ID `123 456 789 012`, password `PASSword`, and create 3 dummy participants.

## Important Notes

- This was tested on Chrome 127 as of August 2024.
- More than 5 test participants is not recommended.

## More Info about Tractus

We make other tools for event production. Check them out [at our website](https://www.tractusevents.com/tools).