export interface UserForLogin {

  email: string;

  password: string;


}

export interface UserLogin {
  id: number;
  email: string;

  name: string;

  token: string;
  /*  role: string;*/


}
