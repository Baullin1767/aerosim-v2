using System.Collections.Generic;
using UnityEngine;

namespace Mission.Missions
{
    public class Images : MonoBehaviour
    {
        [SerializeField]
        private List<ImageItem> images;

        private void Awake()
        {
            foreach (var image in images)
            {
                image.Initialize(ImageClick);
            }

            if (images.Count > 0) images[0].MakeMain();
        }

        private void ImageClick(ImageItem item)
        {
            foreach (var image in images)
            {
                image.MakeMinor();
            }

            item.MakeMain();
        }
    }
}