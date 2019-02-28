# Software Design And Development Practice III
This project is still under development.

## Caution: This is a school project
It's really happy that you spend your time checking this proj out, but unfortunatelly this proj is only a school project.

'A school project' means that this project is completely just a test version , **is not and will not be** well developed.

## Harbin Institute of Technology (Weihai) Campus Card Management System
### Preview
** Attention: Some of the GIFs are quite big. **

#### Dynamic Transitioning Activities
Server-side will get one latest news regularly and synchronize them to the clien-side.

The client will show one latest news with a transitioning text style.

(The effect can be seen below in GIFs.)

#### Login
![Image](https://fileshk.arhshrn.cn/github/devprac3/loginwindow.jpg)<br>

#### Input Check
![Image](https://fileshk.arhshrn.cn/github/devprac3/inputcheck.gif)<br>
Input Check will check if your StudentNumber is valid.

#### Input Cancel & Select-All-On-Focus
![Image](https://fileshk.arhshrn.cn/github/devprac3/inputcancel.gif)<br>
Click "x" button on the right to clear your input, and "Student No." textbox will select all input when it's on focus.

#### Login Cancel
![Image](https://fileshk.arhshrn.cn/github/devprac3/logincancel.gif)<br>
You can click "cancel" button if the connection to the SQL server is bad.

#### Quit Confirmation
![Image](https://fileshk.arhshrn.cn/github/devprac3/quitconfirm.gif)<br>
The App will ask if you are trying to close the app, or you can disable it once at top-right.

# Expected Functionality Frontend
## Login
- SQL Query
- Users' first-use sign-up. (To complete some basic info)
- Forgot (Find Password)

## Users
### User Group (Sort By Authority Ascent)
#### Basic Roles
- Students (Basic use of all features)
- Operation Administrator (Plus management of the data except roles)
- Super Administrator (Plus management of the roles)
#### Additional Roles
- Project Collaborator
### User Info
- Avatars (Opt. using Gravatar)
- Basics
- Identity Tag (Project Collaborators / Students / Admins etc.)

## Campus Card Management
### Balance (Should be made into a DataGrip)
- Current Balance
- Current Transitional Balance
- Last Transitional Balance

# Expected Functionality Backend
## AES256 En/Decryption
- Store encrypted user info on server side.
## Data Synchronization
- Runtime Sync (Pull data when users request to)
- **Periodic Sync (Periodic data-grab from the site. A server side task, shouln't be integrated into the program.)**