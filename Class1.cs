using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace ����ͷ���¼��
{
    ///   <summary>   
    ///   һ����������ͷ����   
    ///   </summary>   
    public class cVideo
    {
        //����API����
        [DllImport("avicap32.dll")]
        private static extern IntPtr capCreateCaptureWindowA(byte[] lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);
        [DllImport("avicap32.dll")]
        private static extern int capGetVideoFormat(IntPtr hWnd, IntPtr psVideoFormat, int wSize);
        [DllImport("User32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        //��������
        private const int WM_USER = 0x400;
        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;
        private const int WM_CAP_START = WM_USER;
        private const int WM_CAP_STOP = WM_CAP_START + 68;
        private const int WM_CAP_DRIVER_CONNECT = WM_CAP_START + 10;
        private const int WM_CAP_DRIVER_DISCONNECT = WM_CAP_START + 11;
        private const int WM_CAP_SAVEDIB = WM_CAP_START + 25;
        private const int WM_CAP_GRAB_FRAME = WM_CAP_START + 60;
        private const int WM_CAP_SEQUENCE = WM_CAP_START + 62;
        private const int WM_CAP_FILE_SET_CAPTURE_FILEA = WM_CAP_START + 20;
        private const int WM_CAP_SEQUENCE_NOFILE = WM_CAP_START + 63;
        private const int WM_CAP_SET_OVERLAY = WM_CAP_START + 51;
        private const int WM_CAP_SET_PREVIEW = WM_CAP_START + 50;
        private const int WM_CAP_SET_CALLBACK_VIDEOSTREAM = WM_CAP_START + 6;
        private const int WM_CAP_SET_CALLBACK_ERROR = WM_CAP_START + 2;
        private const int WM_CAP_SET_CALLBACK_STATUSA = WM_CAP_START + 3;
        private const int WM_CAP_SET_CALLBACK_FRAME = WM_CAP_START + 5;
        private const int WM_CAP_SET_SCALE = WM_CAP_START + 53;
        private const int WM_CAP_SET_PREVIEWRATE = WM_CAP_START + 52;
        private const int WM_CAP_FILE_SAVEAS = WM_CAP_START + 23;   //����׽�ļ�����Ϊ��һ���û�ָ�����ļ��������Ϣ����ı䲶׽�ļ������ֺ�����,

        //ȫ�ֱ���
        private IntPtr hWndC;��//���
        private IntPtr mControlPtr;��//���
        private bool bWorkStart = false;
        private int mWidth;����//��Ƶ��ʾ���
        private int mHeight;   //��Ƶ��ʾ�߶�
        private int mLeft;    //��Ƶ��ʾ��߾�
        private int mTop;     //��Ƶ��ʾ�ϱ߾�
        ///   <summary>   
        ///   ��ʼ����ʾͼ��   
        ///   </summary>   
        ///   <param   name="handle">������Ƶ�ؼ��ľ��</param>   
        ///   <param   name="left">��Ƶ��ʾ����߾�</param>   
        ///   <param   name="top">��Ƶ��ʾ���ϱ߾�</param>   
        ///   <param   name="width">Ҫ��ʾ��Ƶ�Ŀ��</param>   
        ///   <param   name="height">Ҫ��ʾ��Ƶ�ĳ���</param>   
        public cVideo(IntPtr handle, int left, int top, int width, int height)
        {
            mControlPtr = handle;
            mWidth = width;
            mHeight = height;
            mLeft = left;
            mTop = top;
        }
        ///   <summary>   
        ///   ����Ƶ 
        ///   </summary>   
        public void StartVideo()
        {
            if (bWorkStart)
                return;
            bWorkStart = true;
            byte[] lpszName = new byte[100];
            //�������Ŵ���
            hWndC = capCreateCaptureWindowA(lpszName, WS_CHILD | WS_VISIBLE, mLeft, mTop, mWidth, mHeight, mControlPtr, 0);

            if (hWndC.ToInt32() != 0)
            {
                //��ʾ��Ƶ
                SendMessage(hWndC, WM_CAP_SET_CALLBACK_VIDEOSTREAM, 0, 0);
                SendMessage(hWndC, WM_CAP_SET_CALLBACK_ERROR, 0, 0);
                SendMessage(hWndC, WM_CAP_SET_CALLBACK_STATUSA, 0, 0);
                SendMessage(hWndC, WM_CAP_DRIVER_CONNECT, 0, 0);
                SendMessage(hWndC, WM_CAP_SET_SCALE, 1, 0);
                SendMessage(hWndC, WM_CAP_SET_PREVIEWRATE, 100, 0);
                SendMessage(hWndC, WM_CAP_SET_OVERLAY, 1, 0);
                SendMessage(hWndC, WM_CAP_SET_PREVIEW, 1, 0);
            }
        }
        ///   <summary>   
        ///   �ر���Ƶ  
        ///   </summary>   
        public void StopVideo()
        {
            SendMessage(hWndC, WM_CAP_DRIVER_DISCONNECT, 0, 0);
            bWorkStart = false;
        }

        ///   <summary>   
        ///   ��ʼ¼��
        ///   </summary>   
        ///   <param   name="path">Ҫ����¼���·��</param>   
        public void StarKinescope(string path)
        {
            IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
            SendMessage(hWndC, WM_CAP_FILE_SET_CAPTURE_FILEA, 0, hBmp.ToInt32());
            SendMessage(hWndC, WM_CAP_SEQUENCE, 0, 0);
        }
        /// <summary>
        /// ֹͣ¼��
        /// </summary>
        public void StopKinescope()
        {
            SendMessage(hWndC, WM_CAP_STOP, 0, 0);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        public void Images(string path)
        {
            IntPtr imagePath = Marshal.StringToHGlobalAnsi(path);
            SendMessage(hWndC, WM_CAP_SAVEDIB, 0, imagePath.ToInt32()); 
        }

    }
}
