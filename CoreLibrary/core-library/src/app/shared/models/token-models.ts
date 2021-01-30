export interface UserData {
    userName: string;
    firstName: string;
    lastName: string;
}

export interface TokenModel {
    access_token: string;
    expires: number;
    token_type: string;
    requiresPasswordChange: boolean;
    userData: UserData;
    account: string;
}