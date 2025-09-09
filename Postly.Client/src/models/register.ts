export interface RegisterModel {
  ad: string;
  soyad: string;
  email: string;
  sifre: string;
}

export const initializeRegister: RegisterModel = {
  ad: '',
  soyad: '',
  email: '',
  sifre: ''
};
