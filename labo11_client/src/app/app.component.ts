import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PictureService } from './services/picture.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'] // Corrigé de styleUrl à styleUrls
})
export class AppComponent implements OnInit {
  pictureIds: number[] = []; // Liste des IDs des images
  selectedFile: File | null = null; // Pour stocker le fichier sélectionné

  @ViewChild('fileInput') fileInput!: ElementRef; // Référence à l'input file

  constructor(public pictureService: PictureService) {}

  async ngOnInit() {
    this.pictureIds = await this.pictureService.getPictureIds(); // Récupérer les IDs au démarrage
  }

  onFileSelected() {
    const file: File = this.fileInput.nativeElement.files[0];
    if (file) {
      this.selectedFile = file; // Stocke le fichier sélectionné
    }
  }

  async updateDisplay() {
    this.pictureIds = await this.pictureService.getPictureIds(); // Met à jour la liste des IDs
  }

  async postPicture() {
    if (this.selectedFile) {
      let formData = new FormData();
      formData.append('file', this.selectedFile, this.selectedFile.name); // Ajoute le fichier au FormData

      await this.pictureService.postPicture(formData);
      this.updateDisplay(); // Met à jour l'affichage après l'ajout
    } else {
      console.error("Aucun fichier sélectionné.");
    }
  }

  async deletePicture(id: number) {
    await this.pictureService.deletePicture(id);
    this.pictureIds.splice(this.pictureIds.indexOf(id), 1); // Retirer l'ID de la liste
  }
}