#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SocialPlatforms.Impl;
#endif

namespace ExampleProject.Manager
{
#if UNITY_EDITOR
    public class UserDataManagerEditor : EditorWindow
    {
        [MenuItem("Tools/User Data Editor")]
        private static void OpenWindow()
        {
            GetWindow<UserDataManagerEditor>().Show();
            UserData = UserDataManager.UserData ?? new UserData();
        }

        public static UserData UserData;


        private void OnValidate()
        {
            if (UserData != null)
                UserDataManager.SetUserData(UserData);
        }
        void Refresh()
        {
            UserData = UserDataManager.UserData ?? new UserData();
        }
    }
#endif
}