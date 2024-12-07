export const environment = {
  serverUrl: 'https://corelibraryapi.azurewebsites.net/api/',
  serverOrigin: 'https://corelibraryapi.azurewebsites.net/',

  // serverUrl: 'https://localhost:7050/api/',
  // serverOrigin: 'https://localhost:7050',
  swaggerUrl:'swagger/v1/swagger.json',
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
    data: {
      getByTypeId: 'Data/{0}?type={1}',
      getByType: 'Data?type={0}',
      getAll: 'Data/GetAll',
      get: 'Data',
    },
    summary: {
      getParents: 'Summary/GetParents',
      getChilds: 'Summary/GetChilds/{0}',
    },
  },
  production: false,
};
