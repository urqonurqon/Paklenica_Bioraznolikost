using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Novena.Helpers
{
    public static class CanvasGroupHelper
    {
        public static void FadeCanvasGroup(CanvasGroup canvasGroup,bool active, float duration, float delay = 0, Action onCompleteAction = null) {
            if (onCompleteAction != null) {
                if (active) {
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.DOFade(1, duration).SetDelay(delay).OnComplete(() => {
                        canvasGroup.interactable = true;
                        onCompleteAction();
                    });
                }
                else {
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                    canvasGroup.DOFade(0, duration).SetDelay(delay).OnComplete(() => onCompleteAction());
                }
            }
            else {
                if (active) {
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.DOFade(1, duration).SetDelay(delay).OnComplete(() => {
                        canvasGroup.interactable = true;
                    });
                }
                else {
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                    canvasGroup.DOFade(0, duration).SetDelay(delay);
                }
            }
        }
        
        public static void FadeCanvasGroup(List<CanvasGroup> canvasGroupList,bool active, float duration, float delay = 0, Action onCompleteAction = null) {
        //     if (onCompleteAction != null) {
        //         if (active) {
        //             canvasGroup.DOFade(1, duration).SetDelay(delay).OnComplete(() => {
        //                 canvasGroup.interactable = true;
        //                 canvasGroup.blocksRaycasts = true;
        //                 onCompleteAction();
        //             });
        //         }
        //         else {
        //             canvasGroup.interactable = false;
        //             canvasGroup.blocksRaycasts = false;
        //             canvasGroup.DOFade(0, duration).SetDelay(delay).OnComplete(() => onCompleteAction());
        //         }
        //     }
        //     else {
        //         if (active) {
        //             canvasGroup.DOFade(1, duration).SetDelay(delay).OnComplete(() => {
        //                 canvasGroup.interactable = true;
        //                 canvasGroup.blocksRaycasts = true;
        //             });
        //         }
        //         else {
        //             canvasGroup.interactable = false;
        //             canvasGroup.blocksRaycasts = false;
        //             canvasGroup.DOFade(0, duration).SetDelay(delay);
        //         }
        //     }
        }
    }
}
