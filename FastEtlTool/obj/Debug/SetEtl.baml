   M S B A M L   `   `   `  ����  FastEtlTool    FastEtlTool.SetEtl   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $	��600��5      $	��800��6'     � 	Resources  5       �� MergedDictionaries �� Source�� 5	   
    5
      �� 5      $ Css/style.xaml?�6&   %    5      .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��	Bussiness��5      $��0,10,540,14q�6J   $	��200��6_      FrozenColumnCount$ 1��6�    J� RenderTransformOrigin$ 0.846,0.541&�6�   ��6k   ��    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 业务名称��6%      
IsReadOnly .    CanUserSort .     StyleCellText1  +System.Windows.Controls.DataGridBoundColumn	   ElementStyle#	 [  6Z   
   Width5  /System.Windows.Controls.DataGridLengthConverter$	
 200 6�      Binding 6�   ��  �� Path$
 Name� �� UpdateSourceTrigger$ PropertyChanged=�   EditingElementStyle 5      �� 5       �� 
TargetType" ��6    .�5      ��  �� Property � �� �� Value$ Center=�6D   �� 5       � ��$ Center=�6B   �� 5       � ��$	 200��66   5      5      5      �� 5   
   $��更新时间��6   $
��Left=�6   $��
268,20,0,0q�6:   $	��Top=�6N   �� 5   
   -   5        $��
UpdateTime��5   .   $
��Left=�6    �� SelectedIndex$ 0��6@   $��
350,20,0,0q�6R   $	��Top=�6f   $	��180��6~    �� SelectedValuePath$ value��6�    �� DisplayMemberPath$	 key��6�   ��6�   �� 6�   �� 5!   
   $��更新频率��6   $
��Left=�6   $��
268,60,0,0q�6:   $	��Top=�6N   �� 5"   
   -   5        $��	UpdateDay��5"   .   $
��Left=�6   $ 0��6?   $��
350,60,0,0q�6R   $	��Top=�6f   $	��180��6~   $ value��6�   $	 key��6�   ��6�   �� 6�   �� 5$   
   $��抽取条数��6   $
��Left=�6   $��268,100,0,0q�6:   $	��Top=�6O   �� 5%   
   -   5        $��UpdateCount��5%   .   $
��Left=�6   $ 0��6A   $��350,100,0,0q�6S   $	��Top=�6h   $	��180��6�   $ value��6�   $	 key��6�   ��6�   �� 6�   �� 5'   
   $��存放月数��6   $
��Left=�6   $��268,140,0,0q�6:   $	��Top=�6O   �� 5(   
   -   5        $��	DataMonth��5(   .   $
��Left=�6   $ 0��6?   $��350,140,0,0q�6Q   $	��Top=�6f   $	��180��6~   $ value��6�   $	 key��6�   ��6�   �� 6�   �� 5*   
   $��关联主键��6   $
��Left=�6   $��268,180,0,0q�6:   $	��Top=�6O   �� 5+   
   -   5        $��IsDel��5+   .   $
��Left=�6   $ 0��6;   $��350,180,0,0q�6M   $	��Top=�6b   $	��180��6z   $ value��6�   $	 key��6�   ��6�   �� 6�   �� 5.   
   -   5        $��保 存��5.      $��28��6   $
��Left=�6+   $��400,220,0,0q�6F   $	��Top=�6[   $��75��6s   6~   50      51      