import { User } from "./user";

export class UserParams{
    gender: string;
    minAge = 18;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 5;
    orderBy = 'lastActive'


    constructor(user:User){
        if(user.gender==='others'){
            this.gender='others';
        }
        this.gender = user.gender === 'female' ? 'male' : 'female'
    }
}