   M S B A M L   `   `   `  ����  TYW.SDK.Sample)    #TYW.SDK.Sample.Views.SessionControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      �   � ColumnDefinitions  5   
   �� 5      $��2* �6   �� 5	      $��2* �6   �� 5
      $��2* �6   �� 5      $��2* �6    � RowDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��Active account:��6   $��0��6*   $��0��6:   ��5      �� *Resources[Label]+ �� RelativeSource �� *FindAncestor+ �� AncestorType" ��� 5   
   $��1��66   $��0��6F   ��6   ��  �� Path$ AccountName���5      �� *Resources[TextBlock]+ �� *FindAncestor+" ��� 5   
   $
��Edit��6   $��2��6G   $��0��6W   ��6    �� *ViewAccountCommand+��5      �� *Resources[Button]+ �� *FindAncestor+" ��� 5   
   $��
Session ID��6   $��0��6%   $��1��65   ��5      �� *Resources[Label]+ �� *FindAncestor+" ��� 5   
   $��1��62   $��1��6B   ��6   �� $ 	SessionID���5      �� *Resources[TextBox]+ �� *FindAncestor+" ��� 5   
   $��Connect to Session��6   $��2��6X   $��1��6h   ��6.   �� *ConnectSessionCommand+��5      �� *Resources[Button]+ �� *FindAncestor+" ��� 5   
   $��Start new session��6   $��3��6U   $��1��6e   ��6-   �� *StartSessionCommand+��5       �� *Resources[Button]+ �� *FindAncestor+" �5"      5#      