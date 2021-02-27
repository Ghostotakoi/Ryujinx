﻿using Ryujinx.Common.Configuration.Hid;
using Ryujinx.Configuration;
using System;
using System.Collections.Generic;
using static Ryujinx.Common.Configuration.IConfigurationState;

namespace Ryujinx.Common.Configuration
{
    public class GameConfigurationState : IConfigurationState
    {

        /// <summary>
        /// The default configuration instance
        /// </summary>
        public static GameConfigurationState Instance { get; private set; }
        public LoggerSection Logger { get; private set; }
        public SystemSection System { get; private set; }
        public GraphicsSection Graphics { get; private set; }
        public HidSection Hid { get; private set; }

        private HashSet<String> _overrides;

        private GameConfigurationState()
        {
            Logger = new LoggerSection();
            System = new SystemSection();
            Graphics = new GraphicsSection();
            Hid = new HidSection();
            _overrides = new HashSet<string>();
        }

        public bool Override(string configurationName)
        {
            return _overrides.Add(configurationName);
        }

        public bool Overrides(string configurationName)
        {
            return _overrides.Contains(configurationName);
        }

        public GameConfigurationFileFormat ToFileFormat()
        {
            List<ControllerConfig> controllerConfigList = new List<ControllerConfig>();
            List<KeyboardConfig> keyboardConfigList = new List<KeyboardConfig>();

            foreach (InputConfig inputConfig in Hid.InputConfig.Value)
            {
                if (inputConfig is ControllerConfig controllerConfig)
                {
                    controllerConfigList.Add(controllerConfig);
                }
                else if (inputConfig is KeyboardConfig keyboardConfig)
                {
                    keyboardConfigList.Add(keyboardConfig);
                }
            }

            GameConfigurationFileFormat configurationFile = new GameConfigurationFileFormat
            {
                Version = GameConfigurationFileFormat.CurrentVersion,
                ResScale = Graphics.ResScale,
                ResScaleCustom = Graphics.ResScaleCustom,
                MaxAnisotropy = Graphics.MaxAnisotropy,
                AspectRatio = Graphics.AspectRatio,
                GraphicsShadersDumpPath = Graphics.ShadersDumpPath,
                LoggingEnableDebug = Logger.EnableDebug,
                LoggingEnableStub = Logger.EnableStub,
                LoggingEnableInfo = Logger.EnableInfo,
                LoggingEnableWarn = Logger.EnableWarn,
                LoggingEnableError = Logger.EnableError,
                LoggingEnableGuest = Logger.EnableGuest,
                LoggingEnableFsAccessLog = Logger.EnableFsAccessLog,
                LoggingFilteredClasses = Logger.FilteredClasses,
                LoggingGraphicsDebugLevel = Logger.GraphicsDebugLevel,
                EnableFileLog = Logger.EnableFileLog,
                SystemLanguage = System.Language,
                SystemRegion = System.Region,
                SystemTimeZone = System.TimeZone,
                SystemTimeOffset = System.SystemTimeOffset,
                DockedMode = System.EnableDockedMode,
                EnableVsync = Graphics.EnableVsync,
                EnableShaderCache = Graphics.EnableShaderCache,
                EnablePtc = System.EnablePtc,
                EnableFsIntegrityChecks = System.EnableFsIntegrityChecks,
                FsGlobalAccessLogMode = System.FsGlobalAccessLogMode,
                AudioBackend = System.AudioBackend,
                IgnoreMissingServices = System.IgnoreMissingServices,
                EnableKeyboard = Hid.EnableKeyboard,
                Hotkeys = Hid.Hotkeys,
                KeyboardConfig = keyboardConfigList,
                ControllerConfig = controllerConfigList,
                Overrides = _overrides
            };

            return configurationFile;
        }

        public void LoadDefault()
        {
            Graphics.ResScale.Value = GlobalConfigurationState.Instance.Graphics.ResScale.Value;
            Graphics.ResScaleCustom.Value = GlobalConfigurationState.Instance.Graphics.ResScaleCustom.Value;
            Graphics.MaxAnisotropy.Value = GlobalConfigurationState.Instance.Graphics.MaxAnisotropy.Value;
            Graphics.AspectRatio.Value = GlobalConfigurationState.Instance.Graphics.AspectRatio.Value;
            Graphics.ShadersDumpPath.Value = GlobalConfigurationState.Instance.Graphics.ShadersDumpPath.Value;
            Logger.EnableDebug.Value = GlobalConfigurationState.Instance.Logger.EnableDebug.Value;
            Logger.EnableStub.Value = GlobalConfigurationState.Instance.Logger.EnableStub.Value;
            Logger.EnableInfo.Value = GlobalConfigurationState.Instance.Logger.EnableInfo.Value;
            Logger.EnableWarn.Value = GlobalConfigurationState.Instance.Logger.EnableWarn.Value;
            Logger.EnableError.Value = GlobalConfigurationState.Instance.Logger.EnableError.Value;
            Logger.EnableGuest.Value = GlobalConfigurationState.Instance.Logger.EnableGuest.Value;
            Logger.EnableFsAccessLog.Value = GlobalConfigurationState.Instance.Logger.EnableFsAccessLog.Value;
            Logger.FilteredClasses.Value = GlobalConfigurationState.Instance.Logger.FilteredClasses.Value;
            Logger.GraphicsDebugLevel.Value = GlobalConfigurationState.Instance.Logger.GraphicsDebugLevel.Value;
            Logger.EnableFileLog.Value = GlobalConfigurationState.Instance.Logger.EnableFileLog.Value;
            System.Language.Value = GlobalConfigurationState.Instance.System.Language.Value;
            System.Region.Value = GlobalConfigurationState.Instance.System.Region.Value;
            System.TimeZone.Value = GlobalConfigurationState.Instance.System.TimeZone.Value;
            System.SystemTimeOffset.Value = GlobalConfigurationState.Instance.System.SystemTimeOffset.Value;
            System.EnableDockedMode.Value = GlobalConfigurationState.Instance.System.EnableDockedMode.Value;
            Graphics.EnableVsync.Value = GlobalConfigurationState.Instance.Graphics.EnableVsync.Value;
            Graphics.EnableShaderCache.Value = GlobalConfigurationState.Instance.Graphics.EnableShaderCache.Value;
            System.EnablePtc.Value = GlobalConfigurationState.Instance.System.EnablePtc.Value;
            System.EnableFsIntegrityChecks.Value = GlobalConfigurationState.Instance.System.EnableFsIntegrityChecks.Value;
            System.FsGlobalAccessLogMode.Value = GlobalConfigurationState.Instance.System.FsGlobalAccessLogMode.Value;
            System.AudioBackend.Value = GlobalConfigurationState.Instance.System.AudioBackend.Value;
            System.IgnoreMissingServices.Value = GlobalConfigurationState.Instance.System.IgnoreMissingServices.Value;
            Hid.EnableKeyboard.Value = GlobalConfigurationState.Instance.Hid.EnableKeyboard;
            Hid.Hotkeys.Value = GlobalConfigurationState.Instance.Hid.Hotkeys.Value;
            Hid.InputConfig.Value = GlobalConfigurationState.Instance.Hid.InputConfig.Value;
            _overrides = new HashSet<string>();
        }

        public void Load(GameConfigurationFileFormat gameConfigurationFileFormat, string configurationFilePath)
        {
            List<InputConfig> inputConfig = new List<InputConfig>();
            inputConfig.AddRange(gameConfigurationFileFormat.ControllerConfig);
            inputConfig.AddRange(gameConfigurationFileFormat.KeyboardConfig);

            Graphics.ResScale.Value = gameConfigurationFileFormat.ResScale;
            Graphics.ResScaleCustom.Value = gameConfigurationFileFormat.ResScaleCustom;
            Graphics.MaxAnisotropy.Value = gameConfigurationFileFormat.MaxAnisotropy;
            Graphics.AspectRatio.Value = gameConfigurationFileFormat.AspectRatio;
            Graphics.ShadersDumpPath.Value = gameConfigurationFileFormat.GraphicsShadersDumpPath;
            Logger.EnableDebug.Value = gameConfigurationFileFormat.LoggingEnableDebug;
            Logger.EnableStub.Value = gameConfigurationFileFormat.LoggingEnableStub;
            Logger.EnableInfo.Value = gameConfigurationFileFormat.LoggingEnableInfo;
            Logger.EnableWarn.Value = gameConfigurationFileFormat.LoggingEnableWarn;
            Logger.EnableError.Value = gameConfigurationFileFormat.LoggingEnableError;
            Logger.EnableGuest.Value = gameConfigurationFileFormat.LoggingEnableGuest;
            Logger.EnableFsAccessLog.Value = gameConfigurationFileFormat.LoggingEnableFsAccessLog;
            Logger.FilteredClasses.Value = gameConfigurationFileFormat.LoggingFilteredClasses;
            Logger.GraphicsDebugLevel.Value = gameConfigurationFileFormat.LoggingGraphicsDebugLevel;
            Logger.EnableFileLog.Value = gameConfigurationFileFormat.EnableFileLog;
            System.Language.Value = gameConfigurationFileFormat.SystemLanguage;
            System.Region.Value = gameConfigurationFileFormat.SystemRegion;
            System.TimeZone.Value = gameConfigurationFileFormat.SystemTimeZone;
            System.SystemTimeOffset.Value = gameConfigurationFileFormat.SystemTimeOffset;
            System.EnableDockedMode.Value = gameConfigurationFileFormat.DockedMode;
            Graphics.EnableVsync.Value = gameConfigurationFileFormat.EnableVsync;
            Graphics.EnableShaderCache.Value = gameConfigurationFileFormat.EnableShaderCache;
            System.EnablePtc.Value = gameConfigurationFileFormat.EnablePtc;
            System.EnableFsIntegrityChecks.Value = gameConfigurationFileFormat.EnableFsIntegrityChecks;
            System.FsGlobalAccessLogMode.Value = gameConfigurationFileFormat.FsGlobalAccessLogMode;
            System.AudioBackend.Value = gameConfigurationFileFormat.AudioBackend;
            System.IgnoreMissingServices.Value = gameConfigurationFileFormat.IgnoreMissingServices;
            Hid.EnableKeyboard.Value = gameConfigurationFileFormat.EnableKeyboard;
            Hid.Hotkeys.Value = gameConfigurationFileFormat.Hotkeys;
            Hid.InputConfig.Value = inputConfig;
            _overrides = gameConfigurationFileFormat.Overrides;
        }

        public static void Initialize()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("Configuration is already initialized");
            }

            Instance = new GameConfigurationState();
        }
    }
}
