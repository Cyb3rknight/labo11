import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';

const domain = "https://localhost:7180/";

@Injectable({
  providedIn: 'root'
})
export class PictureService {

  constructor(public http : HttpClient) { }

  async postPicture(formData : any){
    const url = `${domain}api/Pictures/PostPicture`; // Remplacez par l'URL correcte pour le téléchargement
    return lastValueFrom(this.http.post(url, formData));
  }

  async getPictureIds(): Promise<number[]> {
    const url = `${domain}api/Pictures/GetPictureIds`; // Ajout du préfixe api/Pictures
    return lastValueFrom(this.http.get<number[]>(url));
}

  async deletePicture(id : number){
    const url = `${domain}api/Pictures/${id}`; // Remplacez par l'URL correcte pour la suppression
    return lastValueFrom(this.http.delete(url));
  }

}
