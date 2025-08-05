using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ImageImporter : MonoBehaviour {
    public static ImageImporter Instance;
    
    private string savedImageFileName = "userImage.png";
    
    
    public List<Texture2D> presetImages;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    /*public void OnImportImageButtonClicked()
    {
        bool hasPermission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

        if (hasPermission)
        {
            OpenGallery();
        }
    }

    private void OpenGallery()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Debug.Log("Selected image path: " + path);
                StartCoroutine(LoadAndCropImage(path));
            }
        }, "Select an image", "image/*");
    }

    private IEnumerator LoadAndCropImage(string path)
    {
        using (WWW www = new WWW("file://" + path))
        {
            yield return www;

            Texture2D originalTexture = www.texture;

            if (originalTexture != null)
            {
                ImageCropper.Instance.Show(originalTexture, (bool result, Texture original, Texture2D croppedImage) =>
                {
                    if (result && croppedImage != null)
                    {
                        string savedPath = SaveCroppedImage(croppedImage);
                        
                        if (!string.IsNullOrEmpty(savedPath))
                        {
                            GameInfoStaticData.userImagePath = savedPath;
                            Debug.Log("Image successfully cropped and saved to: " + savedPath);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Cropping cancelled or failed.");
                    }
                },
                settings: new ImageCropper.Settings()
                {
                    autoZoomEnabled = true,
                    markTextureNonReadable = false,
                    ovalSelection = false,
                    selectionMinAspectRatio = 1f,
                    selectionMaxAspectRatio = 1f
                });
            }
            else
            {
                Debug.LogError("Failed to load image.");
            }
        }
    }

    private string SaveCroppedImage(Texture2D croppedImage)
    {
        //byte[] pngData = croppedImage.EncodeToPNG();
        if (pngData != null)
        {
            string filePath = Path.Combine(Application.persistentDataPath, savedImageFileName);
            File.WriteAllBytes(filePath, pngData);
            return filePath;
        }
        Debug.LogError("Failed to encode image to PNG.");
        return null;
    }
*/
    public Texture2D LoadSavedImage() {
        string filePath = Path.Combine(Application.persistentDataPath, savedImageFileName);
        if (File.Exists(filePath)) {
            byte[] pngBytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(pngBytes);
            return texture;
        }
        Debug.LogWarning("Saved image not found.");
        return null;
    }

    /*public void DeleteSavedImage() {
        string filePath = Path.Combine(Application.persistentDataPath, savedImageFileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Saved image deleted.");
        }
    }*/
    
    public void SavePresetImage(int index) {
        if (presetImages == null || presetImages.Count == 0) {
            Debug.LogWarning("Preset image list is empty.");
            return;
        }

        if (index < 0 || index >= presetImages.Count) {
            Debug.LogWarning("Invalid preset image index.");
            return;
        }

        Texture2D presetImage = presetImages[index];
        /*if (presetImage != null) {
            string savedPath = SaveCroppedImage(presetImage);
           
            if (!string.IsNullOrEmpty(savedPath)) {
                GameInfoStaticData.userImagePath = savedPath;
                Debug.Log("Preset image saved to: " + savedPath);
            }
        } else {
            Debug.LogError("Preset image is null.");
        }*/
    }

}
