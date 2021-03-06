﻿using System;
using ForumDEG.Models;
using ForumDEG.Utils;
using System.Collections.ObjectModel;
using ForumDEG.Interfaces;
using ForumDEG.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace ForumDEG.ViewModels {
    public class CoordinatorMasterPageViewModel : PageService, INotifyPropertyChanged {
        private string _title;
        public string Title {
            get {
                return _title;
            }
            set {
                if (_title != value) {
                    _title = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
                }
            }
        }

        private string _place;
        public string Place {
            get {
                return _place;
            }
            set {
                if (_place != value) {
                    _place = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Place"));
                }
            }
        }

        private string _schedules;
        public string Schedules {
            get {
                return _schedules;
            }
            set {
                if (_schedules != value) {
                    _schedules = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Schedules"));
                }
            }
        }

        private DateTime _date;
        public DateTime Date {
            get {
                return _date;
            }
            set {
                if (_date != value) {
                    _date = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
                }
            }
        }

        private TimeSpan _hour;
        public TimeSpan Hour {
            get {
                return _hour;
            }
            set {
                if (_hour != value) {
                    _hour = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hour"));
                }
            }
        }

        private bool _forumVisibility;
        public bool ForumVisibility {
            get {
                return _forumVisibility;
            }
            set {
                if (_forumVisibility != value) {
                    _forumVisibility = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ForumVisibility"));
                }
            }
        }

        private bool _noForumWarning;
        public bool NoForumWarning {
            get {
                return _noForumWarning;
            }
            set {
                if (_noForumWarning != value) {
                    _noForumWarning = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NoForumWarning"));
                }
            }
        }

        private bool _activityIndicator;
        public bool ActivityIndicator {
            get {
                return _activityIndicator;
            }
            set {
                if (_activityIndicator != value) {
                    _activityIndicator = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActivityIndicator"));
                }
            }
        }

        private bool _isLoaded;
        public bool IsLoaded {
            get {
                return _isLoaded;
            }
            set {
                if (_isLoaded != value) {
                    _isLoaded = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLoaded"));
                }
            }
        }

        private static CoordinatorMasterPageViewModel _instance = null;
        public ObservableCollection<ForumDetailViewModel> Forums { get; private set; }
        public ForumDetailViewModel SelectedForum { get; private set; }
        private readonly IPageService _pageService;
        private readonly Helpers.Forum _forumService;

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand DetailPageCommand { get; set; }

        public CoordinatorMasterPageViewModel(IPageService pageService) {
            ForumVisibility = false;
            NoForumWarning = false;
            ActivityIndicator = false;
            IsLoaded = false;
            _pageService = pageService;
            _forumService = new Helpers.Forum();
            DetailPageCommand = new Command(SeeDetailPage);
        }

        public static CoordinatorMasterPageViewModel GetInstance() {
            if (_instance == null)
                _instance = new CoordinatorMasterPageViewModel(new PageService());
            return _instance;
        }

        public async Task<Forum> SelectNextForum () {
            var listForum = await _forumService.GetForumsAsync();

            return GetLatestForum(listForum);
        }

        public Forum GetLatestForum(List<Forum> forums) {
            ActivityIndicator = true;
            Forum latestForum = null;
            foreach (Forum forum in forums) {
                Debug.WriteLine("[SelectNextForum]: picks a new forum");
                if (DateTime.Compare(DateTime.Today, forum.Date) <= 0) {
                    if (latestForum == null) {
                        latestForum = forum;
                        Debug.WriteLine("[SelectNextForum]: sets a value to the latest forum variable");
                    } else {
                        if (DateTime.Compare(forum.Date, latestForum.Date) < 0) {
                            latestForum = forum;
                            Debug.WriteLine("[SelectNextForum]: updates the next forum");
                        }
                    }
                }
            }
            ActivityIndicator = false;
            return latestForum;
        }

        public async void SelectForum() {
            Debug.WriteLine("[SelectForum]");
            Forum latestForum = null;
            latestForum = await SelectNextForum();

            SetLatestForumFields(latestForum);
        }

        public void SetLatestForumFields(Forum latestForum) {
            if (latestForum == null) {
                ForumVisibility = false;
                NoForumWarning = true;
                Debug.WriteLine("[SelectForum]: noForumWarning is set to: " + _noForumWarning);
            } else {
                ForumVisibility = true;
                NoForumWarning = false;

                Title = latestForum.Title;
                Place = latestForum.Place;
                Schedules = latestForum.Schedules;
                Date = latestForum.Date;
                Hour = latestForum.Hour;

                Debug.WriteLine("[SelectForum]: gets a non null forum");
                Debug.WriteLine("[SelectForum]: title: " + Title);
                Debug.WriteLine("[SelectForum]: place: " + Place);
                Debug.WriteLine("[SelectForum]: schedules: " + Schedules);

                SelectedForum = new ForumDetailViewModel(_pageService) {
                    Title = latestForum.Title,
                    Place = latestForum.Place,
                    Schedules = latestForum.Schedules,
                    Date = latestForum.Date,
                    Hour = latestForum.Hour,
                    RemoteId = latestForum.RemoteId
                };

                IsLoaded = true;

                Debug.WriteLine("[SelectNextForum]: title " + SelectedForum.Title);
                Debug.WriteLine("[SelectNextForum]: place " + SelectedForum.Place);
                Debug.WriteLine("[SelectNextForum]: schedules " + SelectedForum.Schedules);
            }
        }

        public async void SeeDetailPage() {
            Debug.WriteLine("[Coord. Main] - Inside SeeDetailPage");
            await _pageService.PushAsync(new ForumDetailPage(SelectedForum));
        }
    }
}
