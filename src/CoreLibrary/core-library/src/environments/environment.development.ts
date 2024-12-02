export const environment = {
  serverUrl: 'https://localhost:7050/api/',
  // serverUrl: 'https://corelibraryapi.azurewebsites.net/api/',
  serverOrigin: 'https://localhost:7050',
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
    data:{
      getByTypeId:'Data/{0}?type={1}',
      getByType:'Data?type={0}',
      getAll:'Data/GetAll',
      get:'Data'
    }
  },
  production: false,
};
