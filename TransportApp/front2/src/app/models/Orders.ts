export class Orders {
    Id: number;
    StartLat: number;
    StartLng: number;
    EndLat: number;
    EndLng: number;
    IdUser?: number;
    IdDriver?: number;
    IsInProgress: boolean;
    StartDate: Date;

    constructor(id: number, stLt: () => number, stLn: number,endLt: number, endLn: number, IdUser: number, IdDriver: number) {
        this.Id = id;
        this.StartLat = 0;
        this.StartLng = stLn;
        this.EndLat = endLt;
        this.EndLng = endLn;
        this.IdUser = IdUser;
        this.IdDriver = IdDriver;
        this.StartDate = new Date();
        this.IsInProgress = false;
    }

}