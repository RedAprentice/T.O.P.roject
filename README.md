# That One Project
--------------------------------------------
1. Goal

# TRELLO: https://trello.com/b/MyatPLrz/that-one-place
--------------------------------------------
1. Check every Planck Time Unit

# Vision
--------------------------------------------
Make something; thanks.

# How to Use Git(Hub)
--------------------------------------------
Download the GitHub Desktop app. It'll make things a bit easier.

## Getting Started
1. From the application, clone the repo to where you want it.
2. Fetch and pull the repo to make sure you got the repo and the stuff in it.
3. Double check that you actually have some information and files.

## Steps to Take Before You Start a Session
1. Make sure you are on the right branch.
2. Update from master (if applicable).
3. Make a pull to make sure you are up-to-date.
4. Start working.

## After Finishing a Session of Work
1. Commit your work.
2. Give the commit some small description of what happened.
3. Done!

Careful about switching branches. Switching branches in the desktop app might lead to deletion of things that have not been committed on the branch you were previously working on.

## Commits - How to Add to the Repo.
On the desktop app, the bottom right will allow you to make commits to your local branch.
Make sure to push your changes and commits after you are done, so the changes are reflected in the cloud.

## Reverting Commits - OOPS. I made an oopsie.
I don't recommend doing this unless: 1. Everything just broke and you don't know why; 2. Everything still broke, but this time it's a conflict and you don't want to deal with it.

To revert a commit, go to the application and switch to the History tab. Right-click the commit you want reverted and say goodbye to your mistakes.

## Branches - What and How To Use.
Branches are copies of some other branch taken as a specific point in time. A snapshot of another branch.

Branches should be made for features / people. Make a new branch when you are implementing a new feature or when you want to do some experimentation on a personal branch. It doesn't really matter if you use a feature-based or person-based branch system.

Avoid working in a branch that someone is working on. Bonus points for avoiding the same files, which can introduce problems.

Whenever you reach a good checkpoint, make sure to update from master so that you have some of the updates that others made.

## Merging - Recombining Our Work.
When it comes time for you to share your work with everyone else, you will need to merge your branch (that you recently finished committing and pushing to) with another higher branch.

I recommend doing this from the website instead of the app because I personally think it is clearer. For the website, the first one (aka the branch that has an arrow pointing to it) is the one recieving changes.

Note about Rebasing: This is rarely used, but let's say you are implementing a pressure plate system, but the latest movement system broke. You have already made the first version of the pressure plate, but need the changes someone just recently did to fix the movement system. You don't want to merge here because you would lose your work. This is a rare chance to make use of rebasing. 

Note 2: I haven't used rebasing very much, so I'm not well-versed in when to use it and when to not.

## Conflicts - How to (ruin lives and) fix them. 
Please for the love of god, avoid conflicts. This is the worst and most complicated thing to handle in git. Some conflicts might be from meta data, so for those, it is likely needed to add them to the .gitignore

Conflicts will happen in Git when someone changes the same lines as someone else. Git will get confused, and someone will need to go in and manually choose which lines to use. Obviously, this is a pain in the ass. If you get a conflict, try to fix it before making your merge or commit.

Structure to Follow with Branches
--------------------------------------------
Top level: Master Branch
  This level should be stable and should alway run.
  
2nd level: Development Branch
  The branch we will combine changes and work out any bugs that come from that.
  
3rd level: Personal Branch (or feature branch)
  Where you will likely be pushing most of your commits.
  
4th level (optional): Feature Branch*
  If you want more control and have multiple branches for yourself, you can make more branches from your personal one for different purposes. Might be useful for different iterations of the same feature.
