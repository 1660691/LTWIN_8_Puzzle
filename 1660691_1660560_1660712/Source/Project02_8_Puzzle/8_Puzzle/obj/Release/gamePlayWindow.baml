   M S B A M L   `   `   `  ���� C  ?8_Puzzle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:_8_Puzzle	_8_Puzzle      _8_Puzzle.gamePlayWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:_8_Puzzle     -     � Icon$  Resource/puzzle.ico�� � 
ResizeMode$ NoResize=� � Title$ Play 8 Puzzle��$��554.511��$
��1000�� � WindowStartupLocation$ CenterScreen=�.���  � ColumnDefinitions �� $	��525 ��� .+��� $��0��.+��� -   $��board��$	��525��$	��525�����#222�� $��1��$��0 0 0 0q�
���   � J� Opacity$	 0.9Q�.+���  �� Orientation$ 
Horizontal=����#222.+��� -   $��
backButton�� � ToolTip$ Back to main screen��$��10q����#222
�������$��0q�.��Back�� -   $��saveGameButton��$ 	Save game��$��10q����#222
�������$��0q�.��Save�� -   $��
helpButton��$ How to play��$��10q����#222
�������$��0q�.��Help�� $��Resource/stopwatch.png��$��42��$��42��$��0 10 0 0q��� -   $��	timeLabel��$��00:00��$��30(�$
��bold"�$��verdana)�
�������$��Center=�$��0 0 0 0q��� -   $��sampleImage��$��Resource/bg1.jpg��$	��200��$	��200���� -   $��changeImageButton��$��60��$��5q�$��	0 10 0 10q����#222
������� � Cursor$
 Hand��$��0q�.��Change�� .+��� -	   $��	stepLabel��$��Steps:��$��16(�	 �� Left$	 32��

 �� Top$
 23��
��������� -
   $��
stepsValue��$��0��$��16(�$	 77��$
 23��
��������� -   $��
levelLabel��$��Level: ��$��16(�$	 32��$
 50��
��������� -   $��
levelValue��$
��easy��$��16(�$	 78��$
 50��
��������� -   $��upButton��$��30��$��30��$		 354�����#222
�������$
 4��$��0q�.��▲�� -   $��
downButton��$��30��$��30��$
 74��$		 354�����#222
�������$��0q�.��▼�� -   $��
leftButton��$��30��$��30��$
 39��$		 316�����#222
�������$��0q�.��◀�� -   $��rightButton��$��30��$��30��$
 39��$		 392�����#222
�������$��0q�.��▶