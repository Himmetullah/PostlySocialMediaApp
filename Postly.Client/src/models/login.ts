export interface LoginModel{
    email: string;
    sifre: string;
}

export const initializeUser : LoginModel = {
    email: '',
    sifre: ''
}