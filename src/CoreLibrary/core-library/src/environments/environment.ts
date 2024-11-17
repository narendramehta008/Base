export const environment = {
    serverUrl: 'https://corelibraryapi.azurewebsites.net/api/',
    serverOrigin: 'https://corelibraryapi.azurewebsites.net/',
    tokenName: 'tokenDetails',
    apiEndPoint: {
      auth: {
        login: 'auth/login',
        register: 'auth/register',
      },
      user: {
        userLoggedIn: 'user/logged-in',
        profilePic: 'user/0/profile/picture',
      },
      apiManager: {
        get: 'apiManager/Get',
        post: 'apiManager/POst',
      },
    },
    production: true,
  };
  