This SDK is for use in connecting to the Translate Your World International (TYWI) API. 

The TYWI is a JSON web-API required OAuth type authentication. This SDK is written in C#. SDKs for alternative languages may be forthcoming in future; for now, please use this library as a template for connecting using alternative languages, refer to the online API documentation (available via your TYWI account) or contact TYWI support for more information about connecting using your tech stack. 

Accounts
========
You will need to register for an account before you can use the TYWI API. Please register at www.translateyourworld.com. 

Authentication
==============
Translate your world API uses OAuth style authentication, though full OAuth integration is not yet available. Before connecting to a TYWI session you must obtain a valid authentication token.

Sessions
========
TYWI interactions are handled via sessions. Registered users can either create a new session and invite others to connect to an existing session. Both methods are handled via the /session API method.

Devices
=======
When connecting to a session, you must also send a unique identifier. Each device needs to have a unique identifier within the session. The value of this unique identifier is not important and is distinct from any unique identifiers associated with your account. However it helps the software distinguish one connection from another, in the event of anonymous connections without accounts. 

Profiles
========
Once connected to the session, you must also register a profile. Each user has a profile, which indicates their name and the language(s) they are connecting in. 

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