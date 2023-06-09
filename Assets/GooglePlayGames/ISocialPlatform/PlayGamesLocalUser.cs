// <copyright file="PlayGamesLocalUser.cs" company="Google Inc.">
// Copyright (C) 2014 Google Inc.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>

namespace GooglePlayGames
{
    using System;
    using UnityEngine.SocialPlatforms;

    /// <summary>
    /// Represents the Google Play Games local user.
    /// </summary>
    public class PlayGamesLocalUser : PlayGamesUserProfile, ILocalUser
    {
        internal PlayGamesPlatform mPlatform;

        internal PlayGamesLocalUser(PlayGamesPlatform plaf) :
            base("localUser", null, null)
        {
            mPlatform = plaf;
        }

        /// <summary>
        /// Authenticates the local user. Equivalent to calling
        /// <see cref="PlayGamesPlatform.Authenticate" />.
        /// </summary>
        public void Authenticate(Action<bool> callback)
        {
            mPlatform.Authenticate(callback);
        }

        /// <summary>
        /// Authenticates the local user. Equivalent to calling
        /// <see cref="PlayGamesPlatform.Authenticate" />.
        /// </summary>
        public void Authenticate(Action<bool> callback, bool silent)
        {
            mPlatform.Authenticate(callback, silent);
        }

        /// <summary>
        /// Loads all friends of the authenticated user.
        /// </summary>
        public void LoadFriends(Action<bool> callback)
        {
            mPlatform.LoadFriends(this, callback);
        }

        /// <summary>
        /// Not implemented. Returns an empty list.
        /// </summary>
        public IUserProfile[] friends
        {
            get
            {
                return mPlatform.GetFriends();
            }
        }

        /// <summary>
        /// Returns whether or not the local user is authenticated to Google Play Games.
        /// </summary>
        /// <returns>
        /// <c>true</c> if authenticated; otherwise, <c>false</c>.
        /// </returns>
        public bool authenticated
        {
            get
            {
                return mPlatform.IsAuthenticated();
            }
        }

        /// <summary>
        /// Not implemented. As safety placeholder, returns true.
        /// </summary>
        public bool underage
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the display name of the user.
        /// </summary>
        /// <returns>
        /// The display name of the user.
        /// </returns>
        public new string userName
        {
            get
            {
                string retval = string.Empty;
                if (authenticated)
                {
                    retval = mPlatform.GetUserDisplayName();
                    if (!base.userName.Equals(retval))
                    {
                        ResetIdentity(retval, mPlatform.GetUserId(), mPlatform.GetUserImageUrl());
                    }
                }
                return retval;
            }
        }

        /// <summary>
        /// Gets the user's Google id.
        /// </summary>
        /// <returns>
        /// The user's Google id.
        /// </returns>
        public new string id
        {
            get
            {
                string retval = string.Empty;
                if (authenticated)
                {
                    retval = mPlatform.GetUserId();
                    if (!base.id.Equals(retval))
                    {
                        ResetIdentity(mPlatform.GetUserDisplayName(), retval, mPlatform.GetUserImageUrl());
                    }
                }
                return retval;
            }
        }

        /// <summary>
        /// Gets an id token for the user.
        /// </summary>
        /// <returns>
        /// An id token for the user.
        /// </returns>
        public string idToken
        {
            get
            {
                return authenticated ? mPlatform.GetIdToken() : string.Empty;
            }
        }

        /// <summary>
        /// Gets an access token for the user.
        /// </summary>
        /// <returns>
        /// An id token for the user.
        /// </returns>
        public string accessToken
        {
            get
            {
                return authenticated ? mPlatform.GetAccessToken() : string.Empty;
            }
        }

        /// <summary>
        /// Returns true (since this is the local user).
        /// </summary>
        public new bool isFriend
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the local user's state. This is always <c>UserState.Online</c> for
        /// the local user.
        /// </summary>
        public new UserState state
        {
            get
            {
                return UserState.Online;
            }
        }


        public new string AvatarURL
        {
            get
            {
                string retval = string.Empty;
                if (authenticated)
                {
                    retval = mPlatform.GetUserImageUrl();
                    if (!base.id.Equals(retval))
                    {
                        ResetIdentity(mPlatform.GetUserDisplayName(),
                            mPlatform.GetUserId(), retval);
                    }
                }
                return retval;
            }
        }

        /// <summary>
        /// Gets the email of the signed in player.  This is only available
        /// if the web client id is added to the setup (which enables additional
        /// permissions for the application).
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get
            {
                return authenticated ? mPlatform.GetUserEmail() : string.Empty;
            }
        }
    }
}
