import { CommonModule } from '@angular/common';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})
export class TestComponent {
  private fb = inject(FormBuilder); // Inject FormBuilder
  private http = inject(HttpClient); // Inject HttpClient
  myForm!: FormGroup; // Use definite assignment assertion
  selectedFile: File | null = null;
  uploadProgress: number = 0;
  uploadMessage: string = '';
  uploadSuccess: boolean = false;
  isSubmitting: boolean = false;


  ngOnInit(): void {
    this.myForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      description: [''],
    });
  }

  onFileChange(event: any) {
    this.selectedFile = event.target.files[0];
  }

  onSubmit() {
    if (this.myForm.valid && this.selectedFile && !this.isSubmitting) {
      this.isSubmitting = true;
      const formData = new FormData();
      Object.keys(this.myForm.controls).forEach(key => {
        formData.append(key, this.myForm.get(key)!.value);
      });

      formData.append('file', this.selectedFile);

      this.http.post<any>('http://localhost:5170/api/S3Bucket/uploadFiles', formData, {
        reportProgress: true,
        observe: 'events'
      }).subscribe({
        next: (event) => {
          if (event.type === HttpEventType.UploadProgress) {
            this.uploadProgress = Math.round((100 * event.loaded) / (event.total || 1));
          } else if (event.type === HttpEventType.Response) {
            this.uploadMessage = 'Upload successful!';
            this.uploadSuccess = true;
            this.myForm.reset();
            this.selectedFile = null;
          }
        },
        error: (error) => {
          this.uploadMessage = 'Upload failed: ' + error.message;
          this.uploadSuccess = false;
        },
        complete: () => {
          this.uploadProgress = 0;
          this.isSubmitting = false;
        }
      });
    
  }
}}
