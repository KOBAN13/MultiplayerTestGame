using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Factories;
using Services.Interface;
using UI.Base;
using VContainer;

namespace Services
{
    public class ScreenService : IScreenService
    {
        [Inject] private readonly ScreensFactory _screensFactory;
        
        private readonly Dictionary<Type, View> _screensByType = new();
        private readonly LinkedList<View> _screens = new();
        
        public View CurrentScreen { get; private set; }
        private View _loadingScreen;

        public async UniTask<TScreen> OpenAsync<TScreen>() where TScreen : View
        {
            if (_screensByType.TryGetValue(typeof(TScreen), out var screen))
            {
                screen.Open();
                _screens.AddLast(screen);
                CurrentScreen = screen;
                return (TScreen)screen;
            }
            
            var newScreen = await _screensFactory.CreateAsync<TScreen>();
            CloseLoading();
            
            newScreen.Open();
            _screensByType.Add(typeof(TScreen), newScreen);
            _screens.AddLast(newScreen);
            CurrentScreen = newScreen;
    
            return newScreen;
        }

        public void OpenLoading<TScreen>() where TScreen : View
        {
            if (_screensByType.TryGetValue(typeof(TScreen), out var screen))
            {
                _loadingScreen = screen;
            }
            else
            {
                _loadingScreen = _screensFactory.CreateSync<TScreen>();
                _screensByType.Add(typeof(TScreen), _loadingScreen);
            }

            if (_loadingScreen)
            {
                _loadingScreen.Open();
            }
        }

        public void CloseLoading()
        {
            if (_loadingScreen)
            {
                _loadingScreen.Close();
            }
        }

        public void CloseScreen<TScreen>() where TScreen : View
        {
            if (_screensByType.TryGetValue(typeof(TScreen), out var screen))
            {
                _screens.Remove(screen);
                screen.Close();
                screen.gameObject.SetActive(false);
                
                if (_screens.Count > 0)
                    CurrentScreen = _screens.Last.Value;
            }
        }

        public void Close()
        {
            var screen = _screens.Last.Value;

            if (screen == null) 
                return;
            
            _screens.RemoveLast();
            screen.Close();
            screen.gameObject.SetActive(false);
            if (_screens.Count > 0)
            {
                CurrentScreen = screen;
            }
        }
    }
}