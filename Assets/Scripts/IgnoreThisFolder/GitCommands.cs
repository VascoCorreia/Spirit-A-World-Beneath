/*

GIT COMMANDS:

->BRANCHING:

    -> git branch || shows all branches
    
    -> git branch <name> <commit hashcode> || Creates a new branch, on current commit or specified commit

    -> git branch -m <new-name> || Changes branch name

    -> git switch <name> || changes to a new branch, (moves HEAD POINTER)

    -> git push -u origin <local-branch> || uploads a branch to our local repository, -u flag creates a tracking connection 

    -> git branch --track <new-branch-name> <remote-branch-name> || download a remote branch and establishes a trancking connection

    -> git branch -a || see all branches including tracking branches

    -> git branch -v || shows commits that havent been pushed or pulled

    -> git branch -d  <branch-name> || deletes a local branch

    -> git push origin --delete <remote-branch-name> || delete a remote branch

    -> git log <branch1_name>...<branch2_name> || Compares 2 branchess

-> MERGING:

    1) Switch to the branch that I want to have the changes come into

    2) Execute command
        -> git merge <branch-with-changes>


*/