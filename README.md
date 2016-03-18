# hello-world
My first repository
package test1;

import java.io.*;
import java.awt.event.*;
import java.util.*;
import javax.swing.Timer;

import java.awt.*;
import javax.swing.*;

public class Launcher {
	boolean first = true;
	JTextArea text;
	JFrame newrecordframe;
	JFrame recordframe;
	JMenuItem recordmenu1;
	JMenuItem recordmenu2;
	JMenuItem recordmenu3;
	JMenuItem recordmenu4;
	ArrayList<Record> easyrecord = new ArrayList<Record>();
	ArrayList<Record> mediumrecord = new ArrayList<Record>();
	ArrayList<Record> hardrecord = new ArrayList<Record>();
	ArrayList<Record> insanerecord = new ArrayList<Record>();
	int level = 0;
	JRadioButton easybutton;
	JRadioButton mediumbutton;
	JRadioButton hardbutton;
	JRadioButton insanebutton;
	JFrame defaultframe;
	JLabel timelabel1;
	JLabel timelabel2;
	int time = 0;
	Icon flagicon = new ImageIcon("flag.png");
	Icon mineicon = new ImageIcon("mine.png");
	Icon smileicon = new ImageIcon("smile.png");
	Timer clock = new Timer(1000, new clocklistener());
	boolean goon = true;
	int landNum = 0;
	int clickNum = 0;
	boolean firstclick = true;
	JTextField text1;
	JTextField text2;
	JTextField text3;
	JOptionPane jop = new JOptionPane();
	boolean win = true;
	int line = 8;
	int cross = 8;
	int mine = 10;
	JFrame frame;
	JFrame startframe;
	ArrayList<MineButton> minebutton = new ArrayList<MineButton>();

	public static void main(String[] args) {
		Launcher l = new Launcher();
		l.loadrecord();
		l.defaultlaunch();
	}

	public void loadrecord() {
		RecordIO recordio = new RecordIO();
		recordio.recordinput(easyrecord, "easyrecord.txt");
		recordio.recordinput(mediumrecord, "mediumrecord.txt");
		recordio.recordinput(hardrecord, "hardrecord.txt");
		recordio.recordinput(insanerecord, "insanerecord.txt");
	}

	public void defaultlaunch() {
		defaultframe = new JFrame("Clearing Mines");
		easybutton = new JRadioButton("Easy");
		mediumbutton = new JRadioButton("Medium");
		hardbutton = new JRadioButton("Hard");
		insanebutton = new JRadioButton("Insane");
		ButtonGroup buttongroup = new ButtonGroup();
		buttongroup.add(easybutton);
		buttongroup.add(mediumbutton);
		buttongroup.add(hardbutton);
		buttongroup.add(insanebutton);
		Box buttonbox = new Box(BoxLayout.Y_AXIS);
		buttonbox.add(easybutton);
		buttonbox.add(mediumbutton);
		buttonbox.add(hardbutton);
		buttonbox.add(insanebutton);
		//
		JButton start = new JButton("Start!");
		start.addActionListener(new defaultstartListener());
		buttonbox.add(start);
		//
		JPanel panel1 = new JPanel();
		panel1.add(buttonbox);
		defaultframe.add(panel1);
		defaultframe.setSize(300, 300);
		defaultframe.setVisible(true);
	}

	public void launch() {
		startframe = new JFrame("Clearing Mines");
		startframe.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		JPanel panel1 = new JPanel();
		JPanel panel2 = new JPanel();
		JPanel panel3 = new JPanel();
		JPanel mainpanel = new JPanel();
		JLabel label1 = new JLabel("line:");
		JLabel label2 = new JLabel("cross:");
		JLabel label3 = new JLabel("mine numbers:");
		text1 = new JTextField(10);
		text2 = new JTextField(10);
		text3 = new JTextField(10);
		panel1.add(label1);
		panel1.add(text1);
		panel2.add(label2);
		panel2.add(text2);
		panel3.add(label3);
		panel3.add(text3);
		JButton start = new JButton("Start!");
		start.addActionListener(new startlistener());
		Box box = new Box(BoxLayout.Y_AXIS);
		box.add(panel1);
		box.add(panel2);
		box.add(panel3);
		box.add(start);
		mainpanel.add(box);
		startframe.add(mainpanel);
		startframe.setVisible(true);
		startframe.setSize(300, 300);
	}

	public void buildGUI() {
		frame = new JFrame("Clearing Mines");
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		//
		JMenuBar menubar = new JMenuBar();
		JMenu menu1 = new JMenu("Game");
		JMenu menu2 = new JMenu("Record");
		JMenu menu3 = new JMenu("Version");
		JMenuItem Versionmenu = new JMenuItem("Version Info");
		Versionmenu.addActionListener(new VersionListener());
		JMenuItem Gamemenu1 = new JMenuItem("New");
		JMenuItem Gamemenu2 = new JMenuItem("Costom");
		//
		recordmenu1 = new JMenuItem("Easy Record");
		recordmenu2 = new JMenuItem("Medium Record");
		recordmenu3 = new JMenuItem("Hard Record");
		recordmenu4 = new JMenuItem("Insane Record");
		recordmenu1.addActionListener(new ShowRecordListener());
		recordmenu2.addActionListener(new ShowRecordListener());
		recordmenu3.addActionListener(new ShowRecordListener());
		recordmenu4.addActionListener(new ShowRecordListener());
		//
		Gamemenu1.addActionListener(new NewListener());
		Gamemenu2.addActionListener(new CustomListener());
		menu1.add(Gamemenu1);
		menu1.add(Gamemenu2);
		menu2.add(recordmenu1);
		menu2.add(recordmenu2);
		menu2.add(recordmenu3);
		menu2.add(recordmenu4);
		menu3.add(Versionmenu);
		menubar.add(menu1);
		menubar.add(menu2);
		menubar.add(menu3);
		//
		Box timebox = new Box(BoxLayout.X_AXIS);
		timelabel1 = new JLabel("Time:000", JLabel.LEFT);
		JLabel timeicon = new JLabel(smileicon);
		String label2 = new String("Mine:");
		label2 = label2 + mine;
		timelabel2 = new JLabel(label2, JLabel.RIGHT);
		timebox.add(timelabel1);
		timebox.add(timeicon);
		timebox.add(timelabel2);
		Box mainbox = new Box(BoxLayout.Y_AXIS);

		//
		GridLayout grid = new GridLayout(line, cross);
		JPanel panel1 = new JPanel();
		panel1.setLayout(grid);
		int lin = 0;
		int cro = 0;
		Font font = new Font("宋体", 1, 20);
		for (int i = 0; i < line; i++) {
			for (int x = 0; x < cross; x++) {
				MineButton button = new MineButton(lin, cro);
				button.setFont(font);
				button.addMouseListener(new ClickListener());
				panel1.add(button);
				minebutton.add(button);
				cro++;
			}
			lin++;
			cro = 0;
		}
		//
		mainbox.add(timebox);
		mainbox.add(panel1);
		panel1.setSize(50 * cross, 50 * line);
		//
		frame.add(BorderLayout.NORTH, menubar);
		frame.add(BorderLayout.CENTER, mainbox);
		frame.setVisible(true);
		frame.setSize(50 * cross, 65 * line);
	}

	public void writeRecord(ArrayList<Record> record) {
		newrecordframe = new JFrame("New record!");
		JLabel label1 = new JLabel("Amazing!You made a new record", JLabel.LEFT);
		JLabel label2 = new JLabel("Your name:");
		JPanel mainpanel = new JPanel();
		text = new JTextArea(1, 10);
		JButton okbutton = new JButton("OK");
		okbutton.addActionListener(new OkButtonListener());
		JButton cancelbutton = new JButton("Cancel");
		cancelbutton.addActionListener(new CancelButtonListener());
		JPanel panel1 = new JPanel();
		JPanel panel2 = new JPanel();
		Box box = new Box(BoxLayout.Y_AXIS);
		//
		panel1.add(label2);
		panel1.add(text);
		panel2.add(okbutton);
		panel2.add(cancelbutton);
		box.add(panel1);
		box.add(panel2);
		//
		mainpanel.add(BorderLayout.NORTH, label1);
		mainpanel.add(BorderLayout.CENTER, box);
		newrecordframe.add(mainpanel);
		newrecordframe.setSize(300, 300);
		newrecordframe.setVisible(true);

	}

	public void showRecord(ArrayList<Record> record) {
		int count = 1;
		recordframe = new JFrame("Record");
		Box box = new Box(BoxLayout.Y_AXIS);
		//
		for (Record re : record) {
			String num = String.valueOf(count);
			String line = new String(num + ".  " + re.getName() + "     " + re.getTime());
			JLabel label = new JLabel(line);
			box.add(label);
			count++;
		}
		//
		if (record.isEmpty()) {
			JLabel emptylabel = new JLabel("No records to show!");
			box.add(emptylabel);
		}
		//
		JButton closebutton = new JButton("Close");
		closebutton.addActionListener(new CloseListener());
		box.add(closebutton);
		JPanel panel = new JPanel();
		panel.add(box);
		recordframe.add(panel);
		recordframe.setSize(300, 300);
		recordframe.setVisible(true);
	}

	public void setUpGame(int loc) {
		MineButton button;
		int count = mine;
		int location = 0;
		// 设定雷
		while (count > 0) {
			while (true) {
				location = (int) ((Math.random()) * line * cross);
				if (location != loc) {
					button = minebutton.get(location);
					if (!button.getMine()) {
						button.setMine();
						break;
					}
				}
			}
			count--;
		}
		// 设定数字
		int loc1 = 0;
		int loc2 = 0;
		int[] blocation = new int[2];
		for (MineButton button1 : minebutton) {
			blocation = button1.getLocations();
			System.out.println(blocation[0] + " " + blocation[1]);
			loc1 = blocation[0] - 1;
			loc2 = blocation[1] - 1;
			for (int i = 0; i < 3; i++) {
				if (loc1 >= 0 && loc1 < line && loc2 >= 0 && loc2 < cross
						&& minebutton.get(loc1 * cross + loc2).getMine()) {
					button1.numAdd();
				}
				loc2++;
			}
			//
			loc1 = blocation[0];
			loc2 = blocation[1] - 1;
			for (int i = 0; i < 2; i++) {
				if (loc1 >= 0 && loc1 < line && loc2 >= 0 && loc2 < cross
						&& minebutton.get(loc1 * cross + loc2).getMine()) {
					button1.numAdd();
				}
				loc2 = loc2 + 2;
			}
			//
			loc1 = blocation[0] + 1;
			loc2 = blocation[1] - 1;
			for (int i = 0; i < 3; i++) {
				if (loc1 >= 0 && loc1 < line && loc2 >= 0 && loc2 < cross
						&& minebutton.get(loc1 * cross + loc2).getMine()) {
					button1.numAdd();
				}
				loc2++;
			}
			//
		}
	}

	class MineButton extends JButton {
		private boolean clicked = false;
		private int num;
		private boolean mine = false;
		private int location1;
		private int location2;
		private boolean flag = false;

		boolean getflag() {
			return flag;
		}

		void setFlag() {
			if (!this.getClick()) {
				if (!flag) {
					// this.setForeground(Color.RED);
					this.setIcon(flagicon);
					flag = true;
				} else {
					this.setIcon(null);
					flag = false;
				}
			}
		}

		void click() {
			clicked = true;
		}

		boolean getClick() {
			return clicked;
		}

		public MineButton(int loc1, int loc2) {
			location1 = loc1;
			location2 = loc2;
		}

		String getNum() {
			String Num = String.valueOf(num);
			return Num;
		}

		void numAdd() {
			num++;
		}

		void setMine() {
			mine = true;
		}

		boolean getMine() {
			return mine;
		}

		int[] getLocations() {
			int[] location = new int[2];
			location[0] = location1;
			location[1] = location2;
			return location;
		}

		void doclick() {

			if (!this.getClick() && !flag && first) {
				this.click();
				clickNum++;
				if (this.getMine()) {
					clock.stop();
					this.setIcon(mineicon);
					int result = JOptionPane.showConfirmDialog(frame, "Game Over!Try again?", "You lose",
							JOptionPane.YES_NO_OPTION);
					if (result == JOptionPane.YES_OPTION) {
						clickNum = 0;
						firstclick = true;
						frame.dispose();
						minebutton.clear();
						buildGUI();
						goon = false;
					} else {
						firstclick = true;
						frame.dispose();
					}
				} else if (this.getNum().equals("0")) {
					this.setEnabled(false);
					//
					int loc1 = 0;
					int loc2 = 0;
					int[] blocation = new int[2];
					blocation = this.getLocations();
					// System.out.println(blocation[0]+" "+blocation[1]);
					loc1 = blocation[0] - 1;
					loc2 = blocation[1] - 1;
					for (int i = 0; i < 3; i++) {
						if (loc1 >= 0 && loc1 < line && loc2 >= 0 && loc2 < cross) {
							minebutton.get(loc1 * cross + loc2).doclick();
						}
						loc2++;
					}
					//
					loc1 = blocation[0];
					loc2 = blocation[1] - 1;
					for (int i = 0; i < 2; i++) {
						if (loc1 >= 0 && loc1 < line && loc2 >= 0 && loc2 < cross) {
							minebutton.get(loc1 * cross + loc2).doclick();
						}
						loc2 = loc2 + 2;
					}
					//
					loc1 = blocation[0] + 1;
					loc2 = blocation[1] - 1;
					for (int i = 0; i < 3; i++) {
						if (loc1 >= 0 && loc1 < line && loc2 >= 0 && loc2 < cross) {
							minebutton.get(loc1 * cross + loc2).doclick();
						}
						loc2++;
					}
					//
				} else {
					switch (this.getNum()) {
					case "1":
						this.setForeground(Color.BLUE);
						break;
					case "2":
						this.setForeground(Color.GREEN);
						break;
					case "3":
						this.setForeground(Color.RED);
						break;
					case "4":
						this.setForeground(Color.YELLOW);
						break;
					case "5":
						this.setForeground(Color.ORANGE);
						break;
					case "6":
						this.setForeground(Color.PINK);
						break;
					case "7":
						this.setForeground(Color.CYAN);
						break;
					default:
						this.setForeground(Color.GRAY);
						break;
					}
					this.setText(this.getNum());
				}
				if (clickNum >= landNum && first) {
					clock.stop();
					for (MineButton mb : minebutton) {
						if (!mb.getflag()) {
							mb.setFlag();
						}
					}
					System.out.println(first);
					if (level > 0 && first) {
						switch (level) {
						case 1:
							if (easyrecord.size() < 5) {
								writeRecord(easyrecord);
								first = false;
							} else if (time < Integer.parseInt(easyrecord.get(4).getTime())) {
								writeRecord(easyrecord);
								first = false;
							}

							break;
						case 2:
							if (mediumrecord.size() < 5) {
								writeRecord(mediumrecord);
								first = false;
							} else if (time < Integer.parseInt(mediumrecord.get(4).getTime())) {
								writeRecord(mediumrecord);
								first = false;
							}

							break;
						case 3:
							if (hardrecord.size() < 5) {
								writeRecord(hardrecord);
								first = false;
							} else if (time < Integer.parseInt(hardrecord.get(4).getTime())) {
								writeRecord(hardrecord);
								first = false;
							}

							break;
						case 4:
							if (insanerecord.size() < 5) {
								writeRecord(insanerecord);
								first = false;
							} else if (time < Integer.parseInt(insanerecord.get(4).getTime())) {
								writeRecord(insanerecord);
								first = false;
							}

							break;
						}

					}
					if (first) {
						first = false;
						int result = JOptionPane.showConfirmDialog(frame, "Congratulations! Play again?", "You win!",
								JOptionPane.YES_NO_OPTION);

						if (result == JOptionPane.YES_OPTION) {
							clickNum = 0;
							firstclick = true;
							frame.dispose();
							defaultlaunch();
							minebutton.clear();

						}
					}
				}
			}
		}
	}

	class ClickListener implements MouseListener {
		MineButton button;

		public void mouseClicked(MouseEvent e) {
			int mods = e.getModifiers();
			button = (MineButton) e.getSource();
			if (mods == InputEvent.BUTTON1_MASK) {
				if (firstclick) {
					int[] loc = button.getLocations();
					setUpGame(loc[0] * cross + loc[1]);
					landNum = line * cross - mine;
					firstclick = false;
					clock.start();
				}
				button.doclick();
			}
			if (mods == InputEvent.BUTTON3_MASK) {
				button.setFlag();
			}
		}

		public void mousePressed(MouseEvent e) {

		}

		public void mouseReleased(MouseEvent e) {

		}

		public void mouseEntered(MouseEvent e) {
		}

		public void mouseExited(MouseEvent e) {

		}

	}

	class clocklistener implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			time++;
			String stringtime = String.format("Time:%03d", time);
			timelabel1.setText(stringtime);
		}
	}

	class startlistener implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			line = Integer.parseInt(text1.getText());
			cross = Integer.parseInt(text2.getText());
			mine = Integer.parseInt(text3.getText());
			if (mine >= line * cross) {
				JOptionPane.showConfirmDialog(startframe, "The Num of mines is more than lands!", "Error",
						JOptionPane.OK_CANCEL_OPTION);
			} else {
				buildGUI();
				level = 0;
				startframe.dispose();
				first = true;
				time = 0;
			}
		}
	}

	class CustomListener implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			int result = JOptionPane.showConfirmDialog(frame, "You will lose this game!", "Prompt",
					JOptionPane.YES_NO_OPTION);
			if (result == JOptionPane.YES_OPTION) {
				clickNum = 0;
				firstclick = true;
				frame.dispose();
				minebutton.clear();
				launch();
			}
		}
	}

	class NewListener implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			int result = JOptionPane.showConfirmDialog(frame, "You will lose this game!", "Prompt",
					JOptionPane.YES_NO_OPTION);
			if (result == JOptionPane.YES_OPTION) {
				clickNum = 0;
				firstclick = true;
				frame.dispose();
				minebutton.clear();
				defaultlaunch();
			}
		}
	}

	class VersionListener implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			JOptionPane.showMessageDialog(frame, "Mine Cleaner 4.0.0 Version", "Version Infomation",
					JOptionPane.OK_OPTION);
		}
	}

	class ShowRecordListener implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			ArrayList<Record> record = new ArrayList<Record>();
			if (e.getSource().equals(recordmenu1)) {
				record = easyrecord;
			} else if (e.getSource().equals(recordmenu2)) {
				record = mediumrecord;
			} else if (e.getSource().equals(recordmenu3)) {
				record = hardrecord;
			} else {
				record = insanerecord;
			}
			showRecord(record);
		}
	}

	class CloseListener implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			recordframe.dispose();
		}
	}

	class OkButtonListener implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			RecordIO reio = new RecordIO();
			Record re = new Record();
			re.setName(text.getText());
			re.setTime(String.format("%03d", time));
			switch (level) {
			case 1:
				if (easyrecord.size() < 5) {
				} else {
					easyrecord.remove(4);
				}
				easyrecord.add(re);
				Collections.sort(easyrecord);
				reio.recordOutput(easyrecord, "easyrecord.txt");
				break;
			case 2:
				mediumrecord.remove(4);
				mediumrecord.add(re);
				Collections.sort(mediumrecord);
				reio.recordOutput(mediumrecord, "mediumrecord.txt");
				break;
			case 3:
				hardrecord.remove(4);
				hardrecord.add(re);
				Collections.sort(hardrecord);
				reio.recordOutput(hardrecord, "hardrecord.txt");
				break;
			case 4:
				insanerecord.remove(4);
				insanerecord.add(re);
				Collections.sort(insanerecord);
				reio.recordOutput(insanerecord, "insanerecord.txt");
				break;
			}
			newrecordframe.dispose();
		}
	}

	class CancelButtonListener implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			int x = JOptionPane.showConfirmDialog(newrecordframe, "You will lose your new record!", "Prompt",
					JOptionPane.YES_NO_OPTION);
			if (x == JOptionPane.YES_OPTION) {
				newrecordframe.dispose();
			}
		}
	}

	class defaultstartListener implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			first = true;
			if (easybutton.isSelected()) {
				line = 6;
				cross = 6;
				mine = 5;
				buildGUI();
				level = 1;
			} else if (mediumbutton.isSelected()) {
				line = 10;
				cross = 10;
				mine = 15;
				buildGUI();
				level = 2;
			} else if (hardbutton.isSelected()) {
				line = 15;
				cross = 15;
				mine = 30;
				buildGUI();
				level = 3;
			} else if (insanebutton.isSelected()) {
				line = 15;
				cross = 15;
				mine = 60;
				buildGUI();
				level = 4;
			} else {
				JOptionPane.showMessageDialog(defaultframe, "Please choose a level!", "Prompt!",
						JOptionPane.OK_CANCEL_OPTION);
			}
			defaultframe.dispose();
			time = 0;
		}
	}
}

class Record implements Comparable<Record> {
	private String name;
	private String time;

	public String getName() {
		return name;
	}

	public void setName(String n) {
		name = n;
	}

	public String getTime() {
		return time;
	}

	public void setTime(String t) {
		time = t;
	}

	public int compareTo(Record r) {
		int record1 = Integer.parseInt(time);
		int record2 = Integer.parseInt(r.getTime());
		return record1 - record2;
	}
}

class RecordIO {
	public void recordinput(ArrayList<Record> list, String type) {
		String line;
		String[] split = new String[2];
		File file1 = new File(type);
		try {
			FileReader fr = new FileReader(file1);
			BufferedReader br = new BufferedReader(fr);
			while ((line = br.readLine()) != null) {
				split = line.split("/");
				Record re = new Record();
				re.setName(split[0]);
				re.setTime(split[1]);
				list.add(re);
			}
			br.close();
		} catch (Exception ex) {
			ex.printStackTrace();
		}
	}

	public void recordOutput(ArrayList<Record> list, String name) {
		try {
			File file = new File(name);
			FileWriter fw = new FileWriter(file);
			BufferedWriter bw = new BufferedWriter(fw);
			for (Record record : list) {
				bw.write(record.getName() + "/" + record.getTime());
				bw.newLine();
			}
			bw.close();
		} catch (Exception ex) {
			ex.printStackTrace();
		}
	}
}
