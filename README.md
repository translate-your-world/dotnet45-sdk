This SDK is inteneded to help with connecting to the TYWI API. 

Connecting to the TYWI API:

Accounts
========
In ordr to use TYWI, you will need and account. Please register here [Account registration link to follow].

Authentication
==============
Translate your world API uses OAuth style authentication (though full OAuth integration is not yet available). 

Sessions
========
Once you have your authentication token, in order to use TYWI, you need to be connected to a session. You can either create a new session and invite others or connect to an existing session with a session ID. Both methods are handled via the /session API method.

Devices
=======
Each device needs to have a unique identifier within the session. The value of this unique identifier is not important and is distinct from any unique identifiers associated with your account. However it helps the software distinguish one connection from another, in the event of anonymous connections without accounts. 

Profiles
========
Each user has a profile, which indicates their name and the language(s) they are connecting in. 

Text and Audio
==============
Once you have your session, device and profile set up, you can send and receive text and audio from the API. 

Example application flow
========================
1. Authenticate
2. Create Session
3. Generate Device ID
4. Create Profile
5. Start sending Text and Audio

Please see the SDK and SDK example for an overview of exactly how to do this. 