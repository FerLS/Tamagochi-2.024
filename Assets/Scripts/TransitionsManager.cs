using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TransitionsManager : MonoBehaviour
{
    public static TransitionsManager Instance;
    bool onTransition;

    public enum TransitioType
    {
        SideBy,
        SlideUpDown,
    }

    public class Transition
    {
        public TransitioType transitionType;
        public Ease ease;
        public float duration;
        public float waitTime;
        public Action actionInTransition;
        public bool inverse;


        [Header("SlideUpDown")]
        public RectTransform screen;

        public Transition(
            TransitioType transitionType,
            Action actionInTransition,
            Ease ease = Ease.OutQuint,
            float duration = 0.5f,
            float waitTime = 0.3f,
            RectTransform screen = null,
            bool inverse = false
        )
        {
            this.transitionType = transitionType;
            this.ease = ease;
            this.duration = duration;
            this.waitTime = waitTime;
            this.screen = screen;
            this.actionInTransition = actionInTransition;
            this.inverse = inverse;
            if (transitionType == TransitioType.SlideUpDown && screen == null)
            {
                throw new Exception("Screen is required for SlideUpDown transition");
            }
        }
    }

    [Header("Components")]

    [SerializeField]
    private GameObject sideByTransition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public async void DoTransition(Transition transition)
    {
        if (onTransition)
            return;
        onTransition = true;
        switch (transition.transitionType)
        {
            case TransitioType.SideBy:

                sideByTransition.SetActive(true);
                sideByTransition.transform.DOLocalMoveX(-1000 * (transition.inverse ? -1 : 1), 0);
                await sideByTransition
                    .transform.DOLocalMoveX(0, transition.duration)
                    .SetEase(transition.ease)
                    .AsyncWaitForCompletion();
                transition.actionInTransition?.Invoke();
                await Task.Delay((int)(transition.waitTime * 1000));
                await sideByTransition
                    .transform.DOLocalMoveX(1000 * (transition.inverse ? -1 : 1), transition.duration)
                    .SetEase(transition.ease)
                    .AsyncWaitForCompletion();

                break;
            case TransitioType.SlideUpDown:

                transition.screen.transform.SetSiblingIndex(
                    transition.screen.transform.parent.childCount - 2
                );

                await transition
                    .screen.DOLocalMoveY(transition.inverse ? 0 : transition.screen.rect.height, 0)
                    .AsyncWaitForCompletion();
                transition.screen.gameObject.SetActive(true);

                await transition
                    .screen.DOLocalMoveY(transition.inverse ? transition.screen.rect.height : 0, transition.duration)
                    .SetEase(transition.ease)
                    .AsyncWaitForCompletion();
                transition.actionInTransition?.Invoke();
                break;
        }
        onTransition = false;
    }
}
